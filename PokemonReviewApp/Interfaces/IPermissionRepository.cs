using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPermissionRepository
    {
        ICollection<Permission> GetPermissions();
        Permission GetPermissionById(int id);
        bool PermissionExists(int id);
        bool CreatePermission(Permission permission);
        bool UpdatePermission(Permission permission);
        bool DeletePermission(Permission permission);
        bool Save();
    }
}
