using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private DataContext  _context;
        public CategoryRepository(DataContext context) : base(context) 
        {
            _context = context;
        }
        public Category GetCategory(int id)
        {
            return _context.Categories
                .Where(e => e.Id == id)
                .FirstOrDefault();
        }
        public bool SoftDeleteCategory(int categoryId, int userId)
        {
            var entity = _context.Categories
                .FirstOrDefault(c => c.Id == categoryId);

            if (entity == null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedUserId = userId;
            entity.DeletedDateTime = DateTime.Now;
            entity.UpdatedUserId = userId;
            entity.UpdatedDateTime = DateTime.Now;

            return Save();
        }

        public void RestoreCategory(Category category)
        {
            category.IsDeleted = false;
            category.DeletedUserId = null;
            category.DeletedDateTime = null;
            _context.Update(category);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories
                .ToList();
        }
        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories
                .Where(e => e.CategoryId == categoryId)
                .Select(c => c.Pokemon)
                .ToList();

        }
        public bool CategoryExist(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }
        public bool CreateCategory(Category category, int userId)
        {
            //Change Tracking: add, update, modify
            //Connected or Disconnected
            //EntityState.Added = disconnected state
            category.CreatedUserId = userId;
            category.CreatedDateTime = DateTime.Now;
            _context.Add(category);
           
            return Save();
        }

        //public bool DeleteCategory(Category category)
        //{
        //    _context.Remove(category);
        //    return Save();
        //}
        public bool UpdateCategory(Category category, int userId)
        {
            category.UpdatedUserId = userId;
            category.UpdatedDateTime = DateTime.Now;

            _context.Update(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Category GetCategoryIncludingDeleted(int id)
        {
            return _context.Categories
        .IgnoreQueryFilters()
        .FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Category> GetDeletedCategories()
        {
            return _context.Categories
                   .IgnoreQueryFilters()       // soft delete filtresini geçici kapat
                   .Where(c => c.IsDeleted)    // sadece silinmiş olanlar
                   .OrderBy(c => c.Id)
                   .ToList();
        }

        public ICollection<Category> GetCategoriesIncludingDeleted()
        {
            return _context.Categories
                .IgnoreQueryFilters()
                .ToList();
        }
    }
}
