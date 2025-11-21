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
        void SoftDelete(Food food);
        Food GetFoodIncludingDeleted(int id);
        void RestoreFood(Food food);
        ICollection<Food> GetDeletedFoods(); bool Save();
    }
}
