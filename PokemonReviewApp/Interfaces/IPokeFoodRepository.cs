using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokeFoodRepository
    {
       
            ICollection<PokeFood> GetPokeFoods();
            PokeFood GetPokeFood(int pokemonId, int foodId);
            bool CreatePokeFood(PokeFood pokeFood);
            bool DeletePokeFood(PokeFood pokeFood);
            bool UpdatePokeFood(PokeFood pokeFood);
            bool PokeFoodExists(int pokemonId, int foodId);
            bool Save();
    }
}
