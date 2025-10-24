using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokePropertyRepository : IPokePropertiesRepository
    {
        private readonly DataContext _context;

        public PokePropertyRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<PokeProperty> GetPokeProperties()
        {
            return _context.PokeProperties
                .Include(pp => pp.Pokemon)
                .Include(pp => pp.Property)
                .ToList();
        }

        public PokeProperty GetPokeProperty(int pokemonId, int propertyId)
        {
            return _context.PokeProperties
                .Include(pp => pp.Pokemon)
                .Include(pp => pp.Property)
                .FirstOrDefault(pp => pp.PokemonId == pokemonId && pp.PropertyId == propertyId);
        }


        public bool PokePropertyExists(int pokemonId, int propertyId)
        {
            return _context.PokeProperties.Any(pp => pp.PokemonId == pokemonId && pp.PropertyId == propertyId);
        }

        public bool CreatePokeProperty(PokeProperty pokeProperty)
        {
            _context.Add(pokeProperty);
            return Save();
        }

        public bool UpdatePokeProperty(PokeProperty pokeProperty)
        {
            _context.Update(pokeProperty);
            return Save();
        }

        public bool DeletePokeProperty(PokeProperty pokeProperty)
        {
            _context.Remove(pokeProperty);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

  
    }
}
