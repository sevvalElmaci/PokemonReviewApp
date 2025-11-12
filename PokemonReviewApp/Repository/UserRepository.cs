using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUserWithLog(UserLog userlog)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1️⃣ Ana tabloya kullanıcı ekle
                _context.Add(userlog);
                _context.SaveChanges();

                // 2️⃣ Log tablosuna aynı kullanıcıdan kayıt oluştur
                var userLog = new UserLog
                {
                    UserId = userlog.Id,
                    UserName = userlog.UserName,
                    LastName = userlog.LastName,
                    Email = userlog.Email,
                    Phone = userlog.Phone,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = 1 // JWT eklenince otomatik alınacak
                };
                _context.UserLogs.Add(userLog);
                _context.SaveChanges();

                // 3️⃣ Tüm işlemler başarılı → Commit et
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                // Hata varsa her şeyi geri al
                transaction.Rollback();
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return false;
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
