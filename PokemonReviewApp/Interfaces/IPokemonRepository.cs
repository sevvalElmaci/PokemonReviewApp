using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces



{
    public interface IPokemonRepository
    {
        //API CALL

        ICollection<Pokemon> GetPokemons(); 
    }
}
