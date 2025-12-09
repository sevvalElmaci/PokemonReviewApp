using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IFoodRepository
    {
        Food GetFood(int id);
        ICollection<Food> GetFoods();
        ICollection<Food> GetFoodsByPokemon(int pokeId);
        

        bool FoodExists(int id);
        bool CreateFood(Food food, int userId);
        bool UpdateFood(Food food, int userId);
        bool SoftDelete(int foodId, int userId);
        Food GetFoodIncludingDeleted(int id);
        void RestoreFood(Food food);
        ICollection<Food> GetDeletedFoods(); bool Save();
        ICollection<Food> GetFoodsIncludingDeleted();

    }
}
