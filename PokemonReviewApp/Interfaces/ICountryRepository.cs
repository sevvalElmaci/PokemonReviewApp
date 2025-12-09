
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerId);
        bool CountryExists(int id);
        bool CreateCountry(Country country, int userId);
        bool UpdateCountry(Country country, int userId);
        bool SoftDeleteCountry(int countryId, int userId);

        Country GetCountryIncludingDeleted(int id);
        void RestoreCountry(Country country);
        ICollection<Country> GetDeletedCountries();
        ICollection<Country> GetCountriesIncludingDeleted();

        bool Save();

    }
}
