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

        // ACTIVE LIST
        public ICollection<PokeFood> GetPokeFoods()
        {
            return _context.PokeFoods
                .Where(pf => !pf.IsDeleted)
                .Include(pf => pf.Pokemon)
                .Include(pf => pf.Food)
                .ToList();
        }

        // ALL (ACTIVE + DELETED)
        public ICollection<PokeFood> GetPokeFoodsIncludingDeleted()
        {
            return _context.PokeFoods
                .IgnoreQueryFilters()
                .Include(pf => pf.Pokemon)
                .Include(pf => pf.Food)
                .ToList();
        }

        // DELETED LIST
        public ICollection<PokeFood> GetDeletedPokeFoods()
        {
            return _context.PokeFoods
                .IgnoreQueryFilters()
                .Where(pf => pf.IsDeleted)
                .Include(pf => pf.Pokemon)
                .Include(pf => pf.Food)
                .ToList();
        }

        // GET ACTIVE
        public PokeFood GetPokeFood(int pokemonId, int foodId)
        {
            return _context.PokeFoods
                .Include(pf => pf.Pokemon)
                .Include(pf => pf.Food)
                .FirstOrDefault(pf => pf.PokemonId == pokemonId &&
                                      pf.FoodId == foodId &&
                                      !pf.IsDeleted);
        }

        // GET INCLUDING DELETED
        public PokeFood GetPokeFoodIncludingDeleted(int pokemonId, int foodId)
        {
            return _context.PokeFoods
                .IgnoreQueryFilters()
                .Include(pf => pf.Pokemon)
                .Include(pf => pf.Food)
                .FirstOrDefault(pf => pf.PokemonId == pokemonId &&
                                      pf.FoodId == foodId);
        }

        public bool PokeFoodExists(int pokemonId, int foodId)
        {
            return _context.PokeFoods
                .Any(pf => pf.PokemonId == pokemonId &&
                           pf.FoodId == foodId &&
                           !pf.IsDeleted);
        }

        // CREATE
        public bool CreatePokeFood(PokeFood pokeFood, int userId)
        {
            pokeFood.CreatedUserId = userId;
            pokeFood.CreatedDateTime = DateTime.Now;

            pokeFood.UpdatedUserId = userId;
            pokeFood.UpdatedDateTime = DateTime.Now;

            pokeFood.IsDeleted = false;

            _context.PokeFoods.Add(pokeFood);
            return Save();
        }

        // UPDATE
        public bool UpdatePokeFood(PokeFood pokeFood, int userId)
        {
            pokeFood.UpdatedUserId = userId;
            pokeFood.UpdatedDateTime = DateTime.Now;

            _context.PokeFoods.Update(pokeFood);
            return Save();
        }

        // SOFT DELETE
        public bool SoftDeletePokeFood(int pokemonId, int foodId, int userId)
        {
            var entity = GetPokeFoodIncludingDeleted(pokemonId, foodId);
            if (entity == null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedUserId = userId;
            entity.DeletedDateTime = DateTime.Now;

            entity.UpdatedUserId = userId;
            entity.UpdatedDateTime = DateTime.Now;

            return Save();
        }

        // RESTORE (NO AUDIT → SENİN KUR'UN)
        public bool RestorePokeFood(int pokemonId, int foodId)
        {
            var entity = GetPokeFoodIncludingDeleted(pokemonId, foodId);
            if (entity == null)
                return false;

            entity.IsDeleted = false;
            entity.DeletedUserId = null;
            entity.DeletedDateTime = null;

            _context.PokeFoods.Update(entity);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
