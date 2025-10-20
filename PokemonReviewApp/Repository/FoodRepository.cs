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
        public bool CreateFood(Food food)
        {
            _context.Add(food);
            return Save();
        }

        public bool DeleteFood(Food food)
        {
            _context.Remove(food);
            return Save();
        }
        public bool UpdateFood(Food food)
        {
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
    }
}
