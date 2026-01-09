using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Dto.PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public CategoryDetailDto GetCategoryDetail(int id)
        {
            var query =
                from c in _context.Categories
                where c.Id == id && !c.IsDeleted

                join u in _context.Users
                    on c.CreatedUserId equals u.Id
                    into userJoin
                from createdUser in userJoin.DefaultIfEmpty()

                join pc in _context.PokemonCategories
                    on c.Id equals pc.CategoryId
                    into pcJoin
                from pc in pcJoin.DefaultIfEmpty()

                join p in _context.Pokemon
                    on pc.PokemonId equals p.Id
                    into pokemonJoin
                from pokemon in pokemonJoin.DefaultIfEmpty()

                select new
                {
                    Category = c,
                    CreatedUser = createdUser,
                    Pokemon = pokemon
                };

            var rows = query.ToList();
            if (!rows.Any())
                return null;

            var category = rows.First().Category;
            category.CreatedUser = rows.First().CreatedUser;

            category.PokemonCategories = rows
                .Where(x => x.Pokemon != null)
                .GroupBy(x => x.Pokemon.Id)
                .Select(g => new PokemonCategory
                {
                    CategoryId = category.Id,
                    PokemonId = g.Key,
                    Pokemon = g.First().Pokemon
                })
                .ToList();

            return new CategoryDetailDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedBy = category.CreatedUser?.UserName,
                Pokemons = category.PokemonCategories
                    .Select(pc => pc.Pokemon.Name)
                    .ToList()
            };
        }

        public Category GetCategoryEntity(int id)
        {
            return _context.Categories
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

 
        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public ICollection<Category> GetCategoriesIncludingDeleted()
        {
            return _context.Categories
                .IgnoreQueryFilters()
                .ToList();
        }

        public ICollection<Category> GetDeletedCategories()
        {
            return _context.Categories
                .IgnoreQueryFilters()
                .Where(c => c.IsDeleted)
                .ToList();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.Pokemon)
                .ToList();
        }

  
        public bool CreateCategory(Category category, int userId)
        {
            category.CreatedUserId = userId;
            category.CreatedDateTime = DateTime.Now;

            _context.Categories.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category, int userId)
        {
            category.UpdatedUserId = userId;
            category.UpdatedDateTime = DateTime.Now;

            _context.Categories.Update(category);
            return Save();
        }

        public bool SoftDeleteCategory(int categoryId, int userId)
        {
            var category = GetCategoryEntity(categoryId);
            if (category == null)
                return false;

            category.IsDeleted = true;
            category.DeletedUserId = userId;
            category.DeletedDateTime = DateTime.Now;

            return Save();
        }

        public void RestoreCategory(Category category)
        {
            category.IsDeleted = false;
            category.DeletedUserId = null;
            category.DeletedDateTime = null;

            _context.Categories.Update(category);
            Save();
        }

        
        public bool CategoryExist(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }
        public Category GetCategoryIncludingDeleted(int id)
        {
            return _context.Categories
                .IgnoreQueryFilters()      // Soft delete filtresini bypass ediyoruz
                .FirstOrDefault(c => c.Id == id);
        }
        public ICollection<CategoryNewDetailDto> GetAllCategoriesNewDetail()
        {
            return
                (from c in _context.Categories
                 where !c.IsDeleted

                 join cu in _context.Users.IgnoreQueryFilters()
                     on c.CreatedUserId equals cu.Id
                     into catUserJoin
                 from categoryCreatedUser in catUserJoin.DefaultIfEmpty()

                 join pc in _context.PokemonCategories
                     on c.Id equals pc.CategoryId
                     into pcJoin
                 from pc in pcJoin.DefaultIfEmpty()

                 join p in _context.Pokemon
                     on pc.PokemonId equals p.Id
                     into pokeJoin
                 from pokemon in pokeJoin.DefaultIfEmpty()

                 join pu in _context.Users.IgnoreQueryFilters()
                     on pokemon.CreatedUserId equals pu.Id
                     into pokeUserJoin
                 from pokemonCreatedUser in pokeUserJoin.DefaultIfEmpty()

                 select new CategoryNewDetailDto
                 {
                     CategoryName = c.Name,
                     PokemonName = pokemon != null ? pokemon.Name : null,
                     CategoryCreatedUserName = categoryCreatedUser.UserName,
                     PokemonCreatedUserName = pokemonCreatedUser.UserName
                 })
                .ToList();
        }

        public CategoryNewDetailDto GetCategoryNewDetail(int categoryId)
        {
            return
                (from pc in _context.PokemonCategories
                 where pc.CategoryId == categoryId

                 join c in _context.Categories
                     on pc.CategoryId equals c.Id

                 join p in _context.Pokemon
                     on pc.PokemonId equals p.Id

                 join cu in _context.Users
                     on c.CreatedUserId equals cu.Id
                     into catUserJoin
                 from categoryCreatedUser in catUserJoin.DefaultIfEmpty()

                 join pu in _context.Users
                     on p.CreatedUserId equals pu.Id
                     into pokeUserJoin
                 from pokemonCreatedUser in pokeUserJoin.DefaultIfEmpty()

                 select new CategoryNewDetailDto
                 {
                     CategoryName = c.Name,
                     PokemonName = p.Name,
                     CategoryCreatedUserName = categoryCreatedUser.UserName,
                     PokemonCreatedUserName = pokemonCreatedUser.UserName
                 }).FirstOrDefault();
        }



        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
