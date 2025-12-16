using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository 
    {
        //API CALL
        ICollection<Pokemon> GetPokemons(); //INTERFACE function -> retrieves data from the repository
        ICollection<Food> GetFoodsByPokemon(int pokeId);
        bool SoftDeletePokemon(int pokeId, int userId);


        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        bool PokemonExists(int pokeId);
        bool CreatePokemon(int ownerId, int categoryId, int foodId, Pokemon pokemon, int userId);
        bool UpdatePokemon(int pokeId, string name, DateTime birthDate, int userId);

        decimal GetPokemonRating(int pokeId);

        Pokemon GetPokemonIncludingDeleted(int id);
        void RestorePokemon(Pokemon pokemon);
        ICollection<Pokemon> GetDeletedPokemons();
        ICollection<Pokemon> GetOwnerIncludingDeleted();
        PokemonNewDetailDto GetPokemonNewDetail(int id);



        bool CreatePokemonWithLog(int ownerId, int categoryId, int foodId,  Pokemon pokemon, int userId);

      
        bool Save();

    }
}
