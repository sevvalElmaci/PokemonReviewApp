using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IUserRepository
    {
        bool CreateUserWithLog(UserLog userlog);
        bool Save();
    }
}
