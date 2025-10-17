using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository,
            IMapper mapper, IOwnerRepository ownerRepository, ICategoryRepository categoryRepository, IReviewRepository reviewRepository)

        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

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
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
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


            if (!_reviewRepository.GetReviewsForAPokemon(pokeId).Any())
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();
            //gelen input modelle uyumlu mu?
            //[required] bir bölüm açtın ve orası boş kaldı model state error verir
            //[emailAddress] için bir bölüm açtın ve oraya mail formatı girilmedi  model error verir

            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            return Ok(rating);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDtoCreate pokemonCreate)
        {

            if (pokemonCreate == null)
                return BadRequest(ModelState);
            //var pokemons = _pokemonRepository.GetPokemons()
            //.Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
            //.FirstOrDefault();

            //if (pokemons != null)
            //{
            //    ModelState.AddModelError("", "Pokemon already exists");
            //    return StatusCode(422, ModelState);
            //}

            var owner = _ownerRepository.GetOwner(ownerId);
            if (owner == null)
                return NotFound("Owner with that id not found");

            var category = _categoryRepository.GetCategory(catId);
            if (category == null)
                return NotFound("Category with that id not found");



            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);



            if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokeId != updatedPokemon.Id)
                return BadRequest("Error: Those id duo are not matching");

            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonToDelete = _pokemonRepository.GetPokemon(pokeId);

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", " Something went wrong deleting pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }

    }
}

