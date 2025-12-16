using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    // ===========================
    // GETTERS
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

    public User? GetUserByUserName(string username)
    {
        return _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.UserName == username && !u.IsDeleted);
    }

    public User GetUserIncludingDeleted(int id)
    {
        return _context.Users
            .IgnoreQueryFilters()
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Id == id);
    }

    public User GetUserWithRole(int id)
    {
        return _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Id == id && !u.IsDeleted);
    }

    public User GetUserWithRole(string username)
    {
        return _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.UserName == username && !u.IsDeleted);
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

    public bool UserExists(int id)
    {
        return _context.Users.Any(u => u.Id == id && !u.IsDeleted);
    }


    // ===========================
    // CREATE (NO LOG)
    // ===========================

    public bool CreateUser(User user, int createdByUserId)
    {
        user.CreatedUserId = createdByUserId;
        user.CreatedDateTime = DateTime.Now;

        _context.Users.Add(user);
        return Save();
    }


    // ===========================
    // CREATE WITH LOG (Transactional)
    // ===========================

    public User CreateUserWithLog(User user)
    {
        using var tx = _context.Database.BeginTransaction();

        try
        {
            // USER kayıt
            _context.Users.Add(user);
            _context.SaveChanges();

            // LOG kayıt
            var log = new UserLog
            {
                UserId = user.Id,
                UserName = user.UserName,
                Action = "Create",
                CreatedUserId = user.CreatedUserId,
                CreatedDateTime = DateTime.Now,
                IsDeleted = false
            };

            _context.UserLogs.Add(log);
            _context.SaveChanges();

            tx.Commit();
            return user;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }


    // ===========================
    // UPDATE (WITH LOG)
    // ===========================

    public bool UpdateUser(User user, int updatedByUserId)
    {
        user.UpdatedUserId = updatedByUserId;
        user.UpdatedDateTime = DateTime.Now;

        _context.Users.Update(user);

        var log = new UserLog
        {
            UserId = user.Id,
            UserName = user.UserName,
            Action = "Update",
            CreatedUserId = updatedByUserId,
            CreatedDateTime = DateTime.Now,
            IsDeleted = false
        };

        _context.UserLogs.Add(log);

        return Save();
    }


    // ===========================
    // SOFT DELETE (WITH LOG)
    // ===========================

    public bool SoftDeleteUser(User user, int deletedByUserId)
    {
        user.IsDeleted = true;
        user.DeletedUserId = deletedByUserId;
        user.DeletedDateTime = DateTime.Now;

        var log = new UserLog
        {
            UserId = user.Id,
            UserName = user.UserName,
            Action = "Delete",
            CreatedUserId = deletedByUserId,
            CreatedDateTime = DateTime.Now,
            IsDeleted = false
        };

        _context.UserLogs.Add(log);

        return Save();
    }


    // ===========================
    // RESTORE (WITH LOG)
    // ===========================

    public bool RestoreUser(User user)
    {
        user.IsDeleted = false;
        user.DeletedUserId = null;
        user.DeletedDateTime = null;

        var log = new UserLog
        {
            UserId = user.Id,
            UserName = user.UserName,
            Action = "Restore",
            CreatedUserId = user.UpdatedUserId,
            CreatedDateTime = DateTime.Now,
            IsDeleted = false
        };

        _context.UserLogs.Add(log);

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
