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
                .Include(u => u.Role)                     
                .OrderBy(u => u.Id)
                .ToList();
        }
        public ICollection<User> GetDeletedUsers()
        {
            return _context.Users
                .IgnoreQueryFilters()
                .Where(u => u.IsDeleted)
                .Include(u => u.Role)
                .ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users
                .Include(u => u.Role)                     
                .FirstOrDefault(u => u.Id == id && !u.IsDeleted);
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public User CreateUserWithLog(User user)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // 1) User ekle
                _context.Users.Add(user);
                _context.SaveChanges();

                // 2) Log ekle
                var userLog = new UserLog
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    CreatedDateTime = DateTime.Now,
                    CreatedUserId = 1             
                };

                _context.UserLogs.Add(userLog);
                _context.SaveChanges();

                transaction.Commit();
                return user;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool SoftDeleteUser(User user)
        {
            user.IsDeleted = true;
            user.DeletedDateTime = DateTime.Now;
            user.DeletedUserId = 1;
            return Save();

        }

        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id && !u.IsDeleted);
        }

        public User? GetUserByUserName(string userName)
        {
            return _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(u => u.UserName == userName && !u.IsDeleted);
        }

        public User GetUserById(int id)
        {
            return _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(u => u.Id == id && !u.IsDeleted);
        }

        public User GetUserWithRole(int id)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == id && !u.IsDeleted);
        }

        public ICollection<Permission> GetUserPermissions(int userId)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(u => u.Id == userId && !u.IsDeleted);

            if (user?.Role == null)
                return new List<Permission>();

            return user.Role.RolePermissions
                .Select(rp => rp.Permission)
                .ToList();
        }

        public bool RestoreUser (User user)
        {
            user.IsDeleted = false;
            user.DeletedDateTime = null;
            user.DeletedUserId = null;
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public User GetUserIncludingDeleted(int id)
        {
            return _context.Users
                .IgnoreQueryFilters()
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}
