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

        public bool DeleteRole(Role role)
        {
            _context.Remove(role);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
