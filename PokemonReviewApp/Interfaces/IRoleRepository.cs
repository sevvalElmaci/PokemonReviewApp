using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetRoles();
        ICollection<Role> GetDeletedRoles();
        Role GetRoleById(int id);
        Role GetRoleByName(string name);
        Role GetRoleIncludingDeleted(int id);

        bool RoleExists(int id);
        bool RoleExists(string name);

        bool CreateRole(Role role, int userId);
        bool UpdateRole(Role role, int userId);
        bool SoftDeleteRole(Role role, int userId);
        bool RestoreRole(Role role);

        ICollection<Permission> GetPermissionsByRoleId(int roleId);
        bool Save();


    }
}
