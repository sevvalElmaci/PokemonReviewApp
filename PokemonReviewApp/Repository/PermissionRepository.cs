using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using Microsoft.EntityFrameworkCore;

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
                .Include(p => p.RolePermissions)
                .ThenInclude(rp => rp.Role)
                .OrderBy(p => p.Id)
                .ToList();
        }

        public Permission GetPermissionById(int id)
        {
            return _context.Permissions
                .Include(p => p.RolePermissions)
                .ThenInclude(rp => rp.Role)
                .FirstOrDefault(p => p.Id == id);
        }

        public bool PermissionExists(int id)
        {
            return _context.Permissions.Any(p => p.Id == id);
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

        public bool DeletePermission(Permission permission)
        {
            _context.Remove(permission);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
