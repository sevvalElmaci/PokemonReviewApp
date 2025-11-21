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
        public void RestoreCategory(Category category)
        {
            category.IsDeleted = false;
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
                .Select(c => c.Pokemon).ToList();

        }
        public bool CategoryExist(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }
        public bool CreateCategory(Category category)
        {
            //Change Tracking: add, update, modify
            //Connected or Disconnected
            //EntityState.Added = disconnected state
            _context.Add(category);
           
            return Save();
        }

        //public bool DeleteCategory(Category category)
        //{
        //    _context.Remove(category);
        //    return Save();
        //}
        public bool UpdateCategory(Category category)
        {
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
    }
}
