using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;


namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemonWithLog(int ownerId, int categoryId, int foodId, Pokemon pokemon)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var ownerEntity = _context.Owners.FirstOrDefault(o => o.Id == ownerId);
                var categoryEntity = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
                var foodEntity = _context.Foods.FirstOrDefault(f => f.Id == foodId);

                if (ownerEntity == null || categoryEntity == null || foodEntity == null)
                    throw new Exception("Invalid related entity");

                var pokemonOwner = new PokemonOwner { Owner = ownerEntity, Pokemon = pokemon };
                var pokemonCategory = new PokemonCategory { Category = categoryEntity, Pokemon = pokemon };
                var pokeFood = new PokeFood { Food = foodEntity, Pokemon = pokemon };


                _context.Add(pokemonOwner);
                _context.Add(pokeFood);
                _context.Add(pokemonCategory);
                _context.Add(pokemon);
                _context.SaveChanges();

                var pokemonLog = new PokemonLog
                {
                    PokemonId = pokemon.Id,
                    Name = pokemon.Name,
                    OwnerId = ownerEntity.Id,
                    CreatedDateTime = DateTime.Now,
                    CreatedUserId = 1 // JWT gelince değişcek
                };

                _context.PokemonLogs.Add(pokemonLog);
                _context.SaveChanges();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"[Transaction Failed] {ex.Message}");
                return false;
            }

        }

        public bool CreatePokemon(int ownerId, int categoryId, int foodId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Owners
                .Where(a => a.Id == ownerId)
                .FirstOrDefault();
            var category = _context.Categories
                .Where(a => a.Id == categoryId)
                .FirstOrDefault();
            var food = _context.Foods
                .Where(f => f.Id == foodId)
                .FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };
            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };
            _context.Add(pokemonCategory);
            var pokeFood = new PokeFood()
            {
                Food = food,
                Pokemon = pokemon,
            };
            _context.Add(pokeFood);
            return Save();
        }


        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public ICollection<Food> GetFoodsByPokemon(int pokeId)
        {
            return _context.PokeFoods
                .Where(pf => pf.Pokemon.Id == pokeId)
                .Select(pf => pf.Food)
                .ToList();
        }



        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemon
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon
                .Where(p => p.Name == name)
                .FirstOrDefault();
        }
        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews
                .Where(p => p.Pokemon.Id == pokeId);
            if (review.Count() <= 0)
            {
                return 0;

            }
            return ((decimal)review.Sum(r => r.Rating) / review.Count());

        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon
                .OrderBy(p => p.Id)
                .ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon
                .Any(p => p.Id == pokeId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;


        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }

}


