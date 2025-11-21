using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace PokemonReviewApp.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Role> GetRoles()
        {
            return _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .OrderBy(r => r.Id)
                .ToList();
        }
        public ICollection<Role> GetDeletedRoles()
        {
            return _context.Roles
                .IgnoreQueryFilters()
                .Where(r => r.IsDeleted)
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .OrderBy(r => r.Id)
                .ToList();
        }
       

        public ICollection<Permission> GetPermissionsByRoleId(int roleId)
        {
            return _context.Roles
                .Where(r => r.Id == roleId)
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .SelectMany(r => r.RolePermissions.Select(rp => rp.Permission))
                .ToList();
        }


        public Role GetRoleById(int id)
        {
            return _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(r => r.Id == id);
        }

        public bool RoleExists(int id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }

        public bool CreateRole(Role role)
        {
            _context.Add(role);
            return Save();
        }

        public bool UpdateRole(Role role)
        {
            _context.Update(role);
            return Save();
        }


        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public Role GetRoleByName(string name)
        {
            return _context.Roles
                .FirstOrDefault(r => r.RoleName == name);
        }

        public Role GetRoleIncludingDeleted(int id)
        {
            return _context.Roles
                .IgnoreQueryFilters()
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(r => r.Id == id);

        }

        public bool RoleExists(string name)
        {
           return _context.Roles.Any(r => r.RoleName == name);
        }

        public bool SoftDeleteRole(Role role)
        {
            role.IsDeleted = true;
            role.DeletedDateTime = DateTime.Now;
            role.DeletedUserId = 1;
            return Save();
        }

        public bool RestoreRole(Role role)
        {
            role.IsDeleted = false;
            role.DeletedDateTime = null;
            role.DeletedUserId = null;
            return Save();
        }
    }
}
