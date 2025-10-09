using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Security.Cryptography.X509Certificates;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PokemonReviewApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly DataContext context;
        public PokemonController(IPokemonRepository pokemonRepository, DataContext context)
        {
            _pokemonRepository = pokemonRepository;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]

        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);

        }
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound;

            var pokemon = _pokemonRepository.GetPokemon(pokeId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);

            }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {

            if (!_pokemonRepository.PokemonExists(pokeId))
            return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            if(!ModelState.IsValid)
                return BadRequest();

            return Ok(rating);

        }

    }
}

