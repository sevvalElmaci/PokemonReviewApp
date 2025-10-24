using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokePropertiesRepository
    {
        ICollection<PokeProperty> GetPokeProperties();
        PokeProperty GetPokeProperty(int pokemonId, int propertyId);
        bool PokePropertyExists(int pokemonId, int propertyId);
        bool CreatePokeProperty(PokeProperty pokeProperty);
        bool UpdatePokeProperty(PokeProperty pokeProperty);
        bool DeletePokeProperty(PokeProperty pokeProperty);
        bool Save();
    }
}
