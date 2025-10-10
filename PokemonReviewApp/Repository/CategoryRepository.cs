using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Xml.Linq;
    namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext  _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;

            
        }


        public bool CategoryExist(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Category> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Category GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
