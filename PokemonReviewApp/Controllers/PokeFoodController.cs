using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokeFoodController : ControllerBase
    {
        private readonly IPokeFoodRepository _pokeFoodRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public PokeFoodController(
            IPokeFoodRepository pokeFoodRepository,
            IPokemonRepository pokemonRepository,
            IFoodRepository foodRepository,
            IMapper mapper)
        {
            _pokeFoodRepository = pokeFoodRepository;
            _pokemonRepository = pokemonRepository;
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPokeFoods()
        {
            var pokeFoods = _mapper.Map<List<PokeFoodDto>>(_pokeFoodRepository.GetPokeFoods());
            return Ok(pokeFoods);
        }

        [HttpGet("{pokemonId}/{foodId}")]
        public IActionResult GetPokeFood(int pokemonId, int foodId)
        {
            var pokeFood = _pokeFoodRepository.GetPokeFood(pokemonId, foodId);
            if (pokeFood == null)
                return NotFound();

            return Ok(_mapper.Map<PokeFoodDto>(pokeFood));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult CreatePokeFood(PokeFoodDtoCreate pokeFood)
        {
            if (pokeFood == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (!_pokemonRepository.PokemonExists(pokeFood.PokemonId))
                return NotFound($"Pokemon with ID {pokeFood.PokemonId} not found.");

            if (!_foodRepository.FoodExists(pokeFood.FoodId))
                return NotFound($"Food with ID {pokeFood.FoodId} not found.");

            
            if (_pokeFoodRepository.PokeFoodExists(pokeFood.PokemonId, pokeFood.FoodId))
                return BadRequest("This relationship already exists.");

          var pokeFoodMap = _mapper
                .Map<PokeFood>(pokeFood);

            if (!_pokeFoodRepository.CreatePokeFood(pokeFoodMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving the relationship.");
                return StatusCode(500, ModelState);
            }

            return Ok($"Relationship between Pokemon and Food  created successfully.");

        }
        [HttpDelete("{pokemonId}/{foodId}")]
        public IActionResult DeletePokeFood(int pokemonId, int foodId, int quantityId)
        {
            var pokeFood = _pokeFoodRepository.GetPokeFood(pokemonId, foodId);
            if (pokeFood == null)
                return NotFound("Relationship not found");

            if (!_pokeFoodRepository.DeletePokeFood(pokeFood))
                return StatusCode(500, "Something went wrong deleting the relationship");

            return NoContent();
        }
        [HttpPut("{pokemonId:int}/{foodId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokeFood(int pokemonId, int foodId, int quantityId, [FromBody] PokeFoodUpdateDto updatedPF)
        {
            if (updatedPF == null)
                return BadRequest("Invalid request body.");

            var existing = _pokeFoodRepository.GetPokeFood(pokemonId, foodId);
            if (existing == null)
                return NotFound($"Relationship with PokemonId={pokemonId} and FoodId={foodId} not found.");

            if (!_pokemonRepository.PokemonExists(updatedPF.ChangedPokemonId))
                return NotFound($"Pokemon with ID {updatedPF.ChangedPokemonId} not found.");

            if (!_foodRepository.FoodExists(updatedPF.ChangedFoodId))
                return NotFound($"Food with ID {updatedPF.ChangedFoodId} not found.");

            if (_pokeFoodRepository.PokeFoodExists(updatedPF.ChangedPokemonId, updatedPF.ChangedFoodId))
                return BadRequest("This new combination already exists.");

            _pokeFoodRepository.DeletePokeFood(existing);

            var updated = new PokeFood
            {
                PokemonId = updatedPF.ChangedPokemonId,
                FoodId = updatedPF.ChangedFoodId
            };

            if (!_pokeFoodRepository.CreatePokeFood(updated))
                return StatusCode(500, "Something went wrong while updating the relationship.");

            return Ok($"Relationship updated successfully from ({pokemonId},{foodId}) to ({updatedPF.ChangedPokemonId},{updatedPF.ChangedFoodId}).");
        }


    }
}
