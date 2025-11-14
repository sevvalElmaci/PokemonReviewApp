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

        public ICollection<User> GetUsers()
        {
            return _context.Users
                .Include(ur => ur.Role)
                .OrderBy(u => u.Id) //listed with an order
                .ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public User CreateUserWithLog(User user)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1️⃣ Ana tabloya kullanıcı ekle
                _context.Add(user);
                _context.SaveChanges();

                // 2️⃣ Log tablosuna aynı kullanıcıdan kayıt oluştur
                var userLog = new UserLog
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    CreatedDateTime = DateTime.Now,
                    CreatedUserId = 1 // JWT eklenince otomatik alınacak
                };
                _context.UserLogs.Add(userLog);
                _context.SaveChanges();

                // 3️⃣ Tüm işlemler başarılı → Commit et
                transaction.Commit();
                return user;
            }
            catch (Exception ex)
            {
                // Hata varsa her şeyi geri al
                transaction.Rollback();
                Console.WriteLine($"Transaction failed: {ex.Message}");
                throw;
            }
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();

        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}
