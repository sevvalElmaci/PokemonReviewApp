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
        bool PokemonExists(int pokeId);
        bool CreatePokemon(int ownerId, int categoryId, int foodId, Pokemon pokemon);
        bool UpdatePokemon(Pokemon pokemon);

        decimal GetPokemonRating(int pokeId);

        void SoftDeletePokemon(Pokemon pokemon);
        Pokemon GetPokemonIncludingDeleted(int id);
        void RestorePokemon(Pokemon pokemon);
        ICollection<Pokemon> GetDeletedPokemons();


        bool CreatePokemonWithLog(int ownerId, int categoryId, int foodId, Pokemon pokemon);

      
        bool Save();

    }
}
