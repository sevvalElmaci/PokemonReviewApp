using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Country GetCountry(int id)
        {
            return _context.Countries
                .Where(c => c.Id == id)
                .FirstOrDefault();
        }
        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners
                .Where(o => o.Id == ownerId)
                .Select(c => c.Country)
                .FirstOrDefault();
        }
        public ICollection<Country> GetCountries()
        {
            return _context.Countries
                .ToList();
        }
        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return _context.Owners
                .Where(c => c.Country.Id == countryId)
                .ToList();
        }
        public bool CountryExists(int id)
        {
            return _context.Countries
                .Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country, int userId)
        {
            country.CreatedUserId = userId;
            country.CreatedDateTime = DateTime.Now;
            _context.Add(country);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool UpdateCountry(Country country, int userId)
        {
            country.UpdatedUserId = userId;
            country.UpdatedDateTime = DateTime.Now;
            _context.Update(country);
            return Save();

        }

        public Country GetCountryIncludingDeleted(int id)
        {
            return _context.Countries
            .IgnoreQueryFilters()
            .FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Country> GetDeletedCountries()
        {
            return _context.Countries
                .IgnoreQueryFilters()
            .Where(c => c.IsDeleted)
            .OrderBy(c => c.Id)
            .ToList();
        }

        public void RestoreCountry(Country country)
        {
            country.IsDeleted = false;
            country.DeletedUserId = null;
            country.DeletedDateTime = null;
            _context.Countries.Update(country);
        }

        public bool SoftDeleteCountry(int countryId, int userId)
        {
            var entity = _context.Countries
                 .FirstOrDefault(c => c.Id == countryId);

            if (entity == null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedUserId = userId;
            entity.DeletedDateTime = DateTime.Now;
            entity.UpdatedUserId = userId;
            entity.UpdatedDateTime = DateTime.Now;

            return Save();
        }

        public ICollection<Country> GetCountriesIncludingDeleted()
        {
            return _context.Countries
               .IgnoreQueryFilters()
               .ToList();
        }
    }
}
