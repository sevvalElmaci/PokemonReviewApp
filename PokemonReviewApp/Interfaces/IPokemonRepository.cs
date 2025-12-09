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
        bool CreatePokemon(int ownerId, int categoryId, int foodId, Pokemon pokemon, int userId);
        bool UpdatePokemon(Pokemon pokemon, int userId);

        decimal GetPokemonRating(int pokeId);

        void SoftDeletePokemon(Pokemon pokemon, int userId);
        Pokemon GetPokemonIncludingDeleted(int id);
        void RestorePokemon(Pokemon pokemon);
        ICollection<Pokemon> GetDeletedPokemons();
        ICollection<Pokemon> GetOwnerIncludingDeleted();


        bool CreatePokemonWithLog(int ownerId, int categoryId, int foodId,  Pokemon pokemon, int userId);

      
        bool Save();

    }
}
