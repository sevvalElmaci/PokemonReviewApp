
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerId);
        bool CountryExists(int id);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        void SoftDeleteCountry(Country country);

        Country GetCountryIncludingDeleted(int id);
        void RestoreCountry(Country country);
        ICollection<Country> GetDeletedCountries();
        bool Save();

    }
}
