using Microsoft.EntityFrameworkCore;
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

        public bool CreatePokemonWithLog(int ownerId, int categoryId, int foodId, Pokemon pokemon, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var ownerEntity = _context.Owners.FirstOrDefault(o => o.Id == ownerId);
                var categoryEntity = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
                var foodEntity = _context.Foods.FirstOrDefault(f => f.Id == foodId);

                if (ownerEntity == null || categoryEntity == null || foodEntity == null)
                    throw new Exception("Invalid related entity");

                _context.Pokemon.Add(pokemon);
                _context.SaveChanges(); // Pokemon ID burada oluşur

                // 3) RELATION TABLES
                _context.PokemonOwners.Add(new PokemonOwner
                {
                    Owner = ownerEntity,
                    Pokemon = pokemon
                });

                _context.PokemonCategories.Add(new PokemonCategory
                {
                    Category = categoryEntity,
                    Pokemon = pokemon
                });

                _context.PokeFoods.Add(new PokeFood
                {
                    Food = foodEntity,
                    Pokemon = pokemon
                });

                _context.SaveChanges();

                var pokemonLog = new PokemonLog
                {
                    PokemonId = pokemon.Id,
                    Name = pokemon.Name,
                    OwnerId = ownerEntity.Id,
                    CreatedDateTime = DateTime.Now,
                    CreatedUserId = userId // JWT gelince değişTİİİ
                };


                pokemon.CreatedUserId = userId;
                pokemon.CreatedDateTime = DateTime.Now;

                _context.Pokemon.Update(pokemon);
                _context.SaveChanges();

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

        public bool CreatePokemon(int ownerId, int categoryId, int foodId, Pokemon pokemon, int userId)
        {
            pokemon.CreatedUserId = userId;
            pokemon.CreatedDateTime = DateTime.Now;

            _context.Add(pokemon);
            Save();

            var log = new PokemonLog
            {
                PokemonId = pokemon.Id,
                ActionType = "CREATE",
                Name = pokemon.Name,
                OwnerId = ownerId,
                CreatedUserId = userId,
                CreatedDateTime = DateTime.Now
            };

            _context.PokemonLogs.Add(log);
            Save();

            return true;
        }



        public void SoftDeletePokemon(Pokemon pokemon, int userId)
        {
            pokemon.IsDeleted = true;
            pokemon.DeletedDateTime = DateTime.Now;
            pokemon.DeletedUserId = userId;

            var log = new PokemonLog
            {
                PokemonId = pokemon.Id,
                ActionType = "DELETE",
                DeletedUserId = userId,
                DeletedDateTime = DateTime.Now
            };

            _context.PokemonLogs.Add(log);

            _context.Pokemon.Update(pokemon);
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

        public bool UpdatePokemon(Pokemon updatedPokemon, int userId)
        {
            var existing = _context.Pokemon
                .Include(p => p.PokemonOwners)
                .FirstOrDefault(p => p.Id == updatedPokemon.Id);

            if (existing == null)
                return false;

            // 1️⃣ Mevcut değerler
            var oldName = existing.Name;
            var oldBirth = existing.BirthDate;

            // Eğer ileride owner güncellemesi gelecekse
            var oldOwnerId = existing.PokemonOwners.FirstOrDefault()?.OwnerId;

            // 2️⃣ Yeni değerleri set et
            existing.Name = updatedPokemon.Name;
            existing.BirthDate = updatedPokemon.BirthDate;

            existing.UpdatedUserId = userId;
            existing.UpdatedDateTime = DateTime.Now;

            // 3️⃣ ChangeTracker şimdi alınmalı
            var entry = _context.Entry(existing);

            // 4️⃣ Tüm değişiklikleri algıla
            foreach (var prop in entry.Properties)
            {
                if (prop.IsModified)
                {
                    var log = new PokemonLog
                    {
                        PokemonId = existing.Id,
                        ActionType = "UPDATE",
                        ChangedField = prop.Metadata.Name,
                        OldValue = prop.OriginalValue?.ToString(),
                        NewValue = prop.CurrentValue?.ToString(),
                        UpdatedUserId = userId,
                        UpdatedDateTime = DateTime.Now,
                        Name = existing.Name,
                        OwnerId = oldOwnerId
                    };

                    _context.PokemonLogs.Add(log);
                }
            }

            return Save();
        }


            //// 3️⃣ Owner değişikliği varsa ayrıca logla
            //if (oldOwnerId != newOwnerId)
            //{
            //    var log = new PokemonLog
            //    {
            //        PokemonId = existing.Id,
            //        ActionType = "UPDATE",
            //        ChangedField = "OwnerId",
            //        OldValue = oldOwnerId.ToString(),
            //        NewValue = newOwnerId.ToString(),
            //        UpdatedUserId = userId,
            //        UpdatedDateTime = DateTime.Now
            //    };

            //    _context.PokemonLogs.Add(log);
            //}

     


        public Pokemon GetPokemonIncludingDeleted(int id)
        {
            return _context.Pokemon
                            .IgnoreQueryFilters()
                            .Include(p => p.PokemonOwners)
                            .Include(p => p.PokemonCategories)
                            .Include(p => p.PokeFoods)
                            .FirstOrDefault(p => p.Id == id);
        }

        public void RestorePokemon(Pokemon pokemon)
        {
            pokemon.IsDeleted = false;
            pokemon.DeletedUserId = null;
            pokemon.DeletedDateTime = null;
            _context.Pokemon.Update(pokemon);
        }

        public ICollection<Pokemon> GetDeletedPokemons()
        {
            return _context.Pokemon
                            .IgnoreQueryFilters()
                            .Where(p => p.IsDeleted)
                            .OrderBy(p => p.Id)
                            .ToList();
        }

        public ICollection<Pokemon> GetOwnerIncludingDeleted()
        {
            return _context.Pokemon
                                        .IgnoreQueryFilters()
                                        .ToList();

        }

    }
}


