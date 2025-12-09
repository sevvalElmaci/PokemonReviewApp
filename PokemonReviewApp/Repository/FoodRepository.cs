using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private readonly DataContext _context;

        //DataContext -> SQL Server database connection
        // _Context -> local variable of this class

        public FoodRepository(DataContext context)
        {
            _context = context;
        }
        public Food GetFood(int id)
        {
            return _context.Foods
                .Where(f => f.Id == id)
                .FirstOrDefault();
            //first or default returns the first element of a sequence, 
            //or a default value if no element is found.
        }

        public ICollection<Food> GetFoods()
        {
            return _context.Foods
                .ToList();
        }
        public bool FoodExists(int id)
        {
            return _context.Foods.Any(f => f.Id == id);
        }
        public bool CreateFood(Food food, int userId)
        {
            food.CreatedUserId = userId;
            food.CreatedDateTime = DateTime.Now;

            food.UpdatedUserId = userId;
            food.UpdatedDateTime = DateTime.Now;

            food.IsDeleted = false;

            _context.Add(food);
            return Save();
        }

       
        public bool UpdateFood(Food food, int userId)
        {
            food.UpdatedUserId = userId;
            food.UpdatedDateTime = DateTime.Now;
            _context.Update(food);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


        public ICollection<Food> GetFoodsByPokemon(int pokeId)
        {
            return _context.PokeFoods
                            .Where(pf => pf.PokemonId == pokeId)
                            .Select(f => f.Food).ToList();
        }

        public bool SoftDelete(int foodId, int userId)
        {
            var entity = _context.Foods
                   .FirstOrDefault(f => f.Id == foodId);

            if (entity == null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedUserId = userId;
            entity.DeletedDateTime = DateTime.Now;
            entity.UpdatedUserId = userId;
            entity.UpdatedDateTime = DateTime.Now;

            return Save();
        }

        public Food GetFoodIncludingDeleted(int id)
        {
            return _context.Foods
                            .IgnoreQueryFilters()
                            .FirstOrDefault(f => f.Id == id);
        }

        public void RestoreFood(Food food)
        {
            food.IsDeleted = false;
            food.DeletedUserId = null;
            food.DeletedDateTime = null;
            _context.Foods.Update(food);
        }

        public ICollection<Food> GetDeletedFoods()
        {
            return _context.Foods
                            .IgnoreQueryFilters()
                            .Where(f => f.IsDeleted)
                            .OrderBy(f => f.Id)
                            .ToList();
        }

        public ICollection<Food> GetFoodsIncludingDeleted()
        {
            return _context.Foods
                          .IgnoreQueryFilters()
                          .ToList();
        }
    }
}
