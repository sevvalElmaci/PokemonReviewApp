using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IUserRepository
    {
        // ===========================
        // GETTERS
        // ===========================
        ICollection<User> GetUsers();
        ICollection<User> GetDeletedUsers();

        User GetUser(int id);
        User GetUserById(int id);
        User? GetUserByUserName(string userName);
        User GetUserIncludingDeleted(int id);

        User GetUserWithRole(int id);
        User GetUserWithRole(string username);

        ICollection<Permission> GetUserPermissions(int userId);

        bool UserExists(int id);


        // ===========================
        // CREATE
        // ===========================

        User CreateUserWithLog(User user);


        // ===========================
        // UPDATE + DELETE + RESTORE (WITH LOGGING)
        // ===========================


        bool UpdateUser(User user, int updatedByUserId);


        bool SoftDeleteUser(User user, int deletedByUserId);

        bool RestoreUser(User user);

        bool Save();
    }
}
