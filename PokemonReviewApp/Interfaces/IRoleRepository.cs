using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetRoles();
        Role GetRoleById(int id);
        bool RoleExists(int id);
        bool CreateRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(Role role);
        bool Save();
    }
}
