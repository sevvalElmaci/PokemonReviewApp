using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository 
    {
        //API CALL
        ICollection<Pokemon> GetPokemons(); //INTERFACE function -> retrieves data from the repository
        ICollection<Food> GetFoodsByPokemon(int pokeId);

        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokeId);
        bool PokemonExists(int pokeId);
        bool CreatePokemon(int ownerId, int categoryId, int foodId, Pokemon pokemon);
        bool CreatePokemonWithLog(int ownerId, int categoryId, int foodId, Pokemon pokemon);

        bool UpdatePokemon(Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();

    }
}
