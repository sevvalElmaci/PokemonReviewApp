using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPermissionRepository
    {
        ICollection<Permission> GetPermissions();
        Permission GetPermissionById(int id);
        Permission GetPermissionByName(string name);
        bool SoftDeletePermission(Permission permission);
        Permission GetPermissionIncludingDeleted(int id);


        bool PermissionExists(int id);
        bool PermissionExists(string name);

        bool CreatePermission(Permission permission);
        bool UpdatePermission(Permission permission);
        bool RestorePermission(Permission permission);

        ICollection<Permission> GetDeletedPermissions();

        bool Save();
    }
}
