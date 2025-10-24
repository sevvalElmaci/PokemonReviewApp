using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PropertyRepository : IPropertyRepository
    //Implementation of IPropertyRepository

    {
        private readonly DataContext _context;
        public PropertyRepository(DataContext context)
        {
            _context = context; //Dependency Injection
        }
        public Property GetProperty(int id)
        {
            return _context.Properties
            .Where(p => p.Id == id)
                .FirstOrDefault();
        }
        public ICollection<Property> GetProperties()
        {
            return _context.Properties
            .ToList();
        }
        public bool PropertyExists(int id)
        {
            return _context.Properties.Any(p => p.Id == id);
        }
        public bool CreateProperty(Property property)
        {
            _context.Add(property);
            return Save();

        }
        public bool UpdateProperty(Property property)
        {
            _context.Update(property);
                return Save();
        }
        public bool DeleteProperty(Property property)
        {
            _context.Remove(property);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }    
    }
}
