using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DataContext _context;

        public PermissionRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Permission> GetPermissions()
        {
            return _context.Permissions
                .OrderBy(p => p.Id)
                .ToList();
        }

        public Permission GetPermissionById(int id)
        {
            return _context.Permissions
                .FirstOrDefault(p => p.Id == id);
        }

        public Permission GetPermissionByName(string name)
        {
            return _context.Permissions
                .FirstOrDefault(p => p.PermissionName == name);
        }

        public bool PermissionExists(int id)
        {
            return _context.Permissions.Any(p => p.Id == id);
        }

        public bool PermissionExists(string name)
        {
            return _context.Permissions.Any(p => p.PermissionName == name);
        }

        public bool CreatePermission(Permission permission)
        {
            _context.Add(permission);
            return Save();
        }

        public bool UpdatePermission(Permission permission)
        {
            _context.Update(permission);
            return Save();
        }

        public bool SoftDeletePermission(Permission permission)
        {
            permission.IsDeleted = true;
            permission.DeletedDateTime = DateTime.Now;
            permission.DeletedUserId = 1; // current user id eklenecek
            _context.Permissions.Update(permission);

            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public bool RestorePermission(Permission permission)
        {
            permission.IsDeleted = false;
            permission.DeletedDateTime = null;
            permission.DeletedUserId = null;
            return Save();
        }

        public ICollection<Permission> GetDeletedPermissions()
        {
            return _context.Permissions
                .IgnoreQueryFilters()
                   .Where(p => p.IsDeleted)
                   .OrderBy(p => p.Id)
                   .ToList();
        }
        public Permission GetPermissionIncludingDeleted(int id)
        {
            return _context.Permissions
                .IgnoreQueryFilters()
                .FirstOrDefault(p => p.Id == id);
        }

    }
}
