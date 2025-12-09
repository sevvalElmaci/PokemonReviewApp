using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        // ============================================
        // GET ACTIVE ROLES
        // ============================================
        public ICollection<Role> GetRoles()
        {
            return _context.Roles
                .Where(r => !r.IsDeleted)
                .ToList();
        }

        // ============================================
        // GET DELETED ROLES
        // ============================================
        public ICollection<Role> GetDeletedRoles()
        {
            return _context.Roles
                .IgnoreQueryFilters()
                .Where(r => r.IsDeleted)
                .ToList();
        }

        // ============================================
        // GET BY ID (Active only)
        // ============================================
        public Role GetRoleById(int id)
        {
            return _context.Roles
                .FirstOrDefault(r => r.Id == id && !r.IsDeleted);
        }

        // ============================================
        // GET BY ID (Including Deleted)
        // ============================================
        public Role GetRoleIncludingDeleted(int id)
        {
            return _context.Roles
                .IgnoreQueryFilters()
                .FirstOrDefault(r => r.Id == id);
        }

        // ============================================
        // GET ROLE BY NAME
        // ============================================
        public Role GetRoleByName(string name)
        {
            return _context.Roles
                .FirstOrDefault(r => r.RoleName.ToLower() == name.ToLower() && !r.IsDeleted);
        }

        // ============================================
        // EXISTENCE CHECK
        // ============================================
        public bool RoleExists(int id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }

        public bool RoleExists(string name)
        {
            return _context.Roles.Any(r => r.RoleName.ToLower() == name.ToLower());
        }

        // ============================================
        // CREATE ROLE (AUDIT INCLUDED)
        // ============================================
        public bool CreateRole(Role role, int userId)
        {
            role.CreatedUserId = userId;
            role.CreatedDateTime = DateTime.Now;
            role.UpdatedUserId = userId;
            role.UpdatedDateTime = DateTime.Now;
            role.IsDeleted = false;

            _context.Roles.Add(role);
            return Save();
        }

        // ============================================
        // UPDATE ROLE (AUDIT INCLUDED)
        // ============================================
        public bool UpdateRole(Role role, int userId)
        {
            role.UpdatedUserId = userId;
            role.UpdatedDateTime = DateTime.Now;

            _context.Roles.Update(role);
            return Save();
        }

        // ============================================
        // SOFT DELETE ROLE
        // ============================================
        public bool SoftDeleteRole(Role role, int userId)
        {
            role.IsDeleted = true;
            role.DeletedUserId = userId;
            role.DeletedDateTime = DateTime.Now;

            role.UpdatedUserId = userId;
            role.UpdatedDateTime = DateTime.Now;

            _context.Roles.Update(role);
            return Save();
        }

        // ============================================
        // RESTORE ROLE (NO AUDIT CHANGES – as you wanted)
        // ============================================
        public bool RestoreRole(Role role)
        {
            role.IsDeleted = false;
            role.DeletedUserId = null;
            role.DeletedDateTime = null;

            _context.Roles.Update(role);
            return Save();
        }

        // ============================================
        // GET PERMISSIONS OF A ROLE
        // ============================================
        public ICollection<Permission> GetPermissionsByRoleId(int roleId)
        {
            return _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToList();
        }

        // ============================================
        // SAVE
        // ============================================
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
