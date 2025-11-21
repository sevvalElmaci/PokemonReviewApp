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

        bool CreateRole(Role role);
        bool UpdateRole(Role role);
        bool SoftDeleteRole(Role role);
        bool RestoreRole(Role role);

        ICollection<Permission> GetPermissionsByRoleId(int roleId);
        bool Save();


    }
}
