using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace PokemonReviewApp.Repository
{
    public class PokeFoodRepository : IPokeFoodRepository
    {
        private readonly DataContext _context;
        public PokeFoodRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<PokeFood> GetPokeFoods()
        {
            return _context.PokeFoods
                .Include(pf => pf.Pokemon)
                .Include(pf => pf.Food)
                .ToList();
        }

        public PokeFood GetPokeFood(int pokemonId, int foodId)
        {
            return _context.PokeFoods
                .Include(pf => pf.Pokemon)
                .Include(pf => pf.Food)
                .FirstOrDefault(pf => pf.PokemonId == pokemonId && pf.FoodId == foodId);
        }

        public bool CreatePokeFood(PokeFood pokeFood)
        {
            _context.PokeFoods.Add(pokeFood);
            return Save();
        }

        public bool DeletePokeFood(PokeFood pokeFood)
        {
            _context.PokeFoods.Remove(pokeFood);
            return Save();
        }

        public bool UpdatePokeFood(PokeFood pokeFood)
        {
            var existing = _context.PokeFoods
                .FirstOrDefault(pf => pf.PokemonId == pokeFood.PokemonId && pf.FoodId == pokeFood.FoodId);

            if(existing == null)
            {
                return false;
            }
            else
            {
                // 🆕 Quantity’yi güncelle
                existing.Quantity = pokeFood.Quantity;

                _context.PokeFoods.Update(existing);
            }
         
            return Save();
        }

        public bool PokeFoodExists(int pokemonId, int foodId)
        {
            return _context.PokeFoods
                .Any(pf => pf.PokemonId == pokemonId && pf.FoodId == foodId);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        
    }
}
