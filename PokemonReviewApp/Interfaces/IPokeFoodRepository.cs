using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokeFoodRepository
    {

        ICollection<PokeFood> GetPokeFoods();
        ICollection<PokeFood> GetPokeFoodsIncludingDeleted();
        ICollection<PokeFood> GetDeletedPokeFoods();

        PokeFood GetPokeFood(int pokemonId, int foodId);
        PokeFood GetPokeFoodIncludingDeleted(int pokemonId, int foodId);

        bool PokeFoodExists(int pokemonId, int foodId);

        bool CreatePokeFood(PokeFood pokeFood, int userId);
        bool UpdatePokeFood(PokeFood pokeFood, int userId);

        bool SoftDeletePokeFood(int pokemonId, int foodId, int userId);
        bool RestorePokeFood(int pokemonId, int foodId);

        bool Save();
    }
}
