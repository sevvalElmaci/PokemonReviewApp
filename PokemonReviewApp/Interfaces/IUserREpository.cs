using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User? GetUserByUserName(string userName);
        User GetUserIncludingDeleted(int id);


        User GetUserById(int id);
        bool UserExists(int id);
        User CreateUserWithLog(User user); //transactional. it effects both two table. userlog and users
        bool CreateUser(User user, int userId); // it just add data not logging.
        bool UpdateUser(User user, int userId);
        bool SoftDeleteUser(User user, int userId);
        bool RestoreUser(User user);

        bool Save();
        User GetUserWithRole(int id);
        ICollection<Permission> GetUserPermissions(int userId);
        User GetUserWithRole(string username);

        ICollection<User> GetDeletedUsers();



    }
}
