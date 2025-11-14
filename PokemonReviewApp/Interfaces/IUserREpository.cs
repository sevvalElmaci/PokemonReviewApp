using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        bool UserExists(int id);
        User CreateUserWithLog(User user); //transactional. it effects both two table. userlog and users
        bool CreateUser(User user); // it just add data not logging.
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
