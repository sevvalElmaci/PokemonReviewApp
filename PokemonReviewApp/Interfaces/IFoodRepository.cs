using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IFoodRepository
    {
        Food GetFood(int id);
        ICollection<Food> GetFoods();
        ICollection<Food> GetFoodsByPokemon(int pokeId);
        

        bool FoodExists(int id);
        bool CreateFood(Food food);
        bool UpdateFood(Food food);
        bool DeleteFood(Food food);
        bool Save();
    }
}
