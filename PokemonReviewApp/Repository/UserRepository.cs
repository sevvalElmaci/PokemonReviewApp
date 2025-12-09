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

        // ===========================
        // BASIC GETTERS
        // ===========================
        public ICollection<User> GetUsers()
        {
            return _context.Users
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted)
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
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(u => u.Id == id);
        }

        
        public User GetUserWithRole(string username)
        {
            return _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(u => u.UserName == username);
        }

        public User? GetUserByUserName(string userName)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == userName && !u.IsDeleted);
        }

        public User GetUserIncludingDeleted(int id)
        {
            return _context.Users
                .IgnoreQueryFilters()
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == id);
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id && !u.IsDeleted);
        }

        public ICollection<Permission> GetUserPermissions(int userId)
        {
            var roleId = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.RoleId)
                .FirstOrDefault();

            return _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToList();
        }

        // ===========================
        // CREATE WITH LOG (TRANSACTIONAL)
        // ===========================
        public User CreateUserWithLog(User user)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // Insert main user
                _context.Users.Add(user);
                _context.SaveChanges();

                // Insert log
                var log = new UserLog
                {
                    UserId = user.Id,
                    CreatedUserId = user.CreatedUserId,
                    CreatedDateTime = DateTime.Now,
                };

                _context.UserLogs.Add(log);
                _context.SaveChanges();

                transaction.Commit();
                return user;
            }
            catch
            {
                transaction.Rollback();
                return null;
            }
        }


        // ===========================
        // CREATE WITHOUT LOG (AUDIT OK)
        // ===========================
        public bool CreateUser(User user, int userId)
        {
            user.CreatedUserId = userId;
            user.CreatedDateTime = DateTime.Now;
            user.UpdatedUserId = userId;
            user.UpdatedDateTime = DateTime.Now;
            user.IsDeleted = false;

            _context.Users.Add(user);
            return Save();
        }

        // ===========================
        // UPDATE (AUDIT OK)
        // ===========================
        public bool UpdateUser(User user, int userId)
        {
            user.UpdatedUserId = userId;
            user.UpdatedDateTime = DateTime.Now;

            _context.Users.Update(user);
            return Save();
        }

        // ===========================
        // SOFT DELETE (AUDIT OK)
        // ===========================
        public bool SoftDeleteUser(User user, int userId)
        {
            user.IsDeleted = true;
            user.DeletedUserId = userId;
            user.DeletedDateTime = DateTime.Now;

            return Save();
        }

        // ===========================
        // RESTORE (NO AUDIT ON PURPOSE)
        // ===========================
        public bool RestoreUser(User user)
        {
            user.IsDeleted = false;
            user.DeletedUserId = null;
            user.DeletedDateTime = null;

            // UpdatedUserId is untouched (as you wanted)
            _context.Users.Update(user);
            return Save();
        }

        // ===========================
        // SAVE
        // ===========================
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
