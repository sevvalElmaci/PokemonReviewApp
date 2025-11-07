using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokeFoodController : Controller
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

        // ✅ GET: /api/PokeFood
        [HttpGet]
        [ProducesResponseType(200)]
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
                return NotFound("Relationship not found.");

            return Ok(_mapper.Map<PokeFoodDto>(pokeFood));
        }

        // ✅ POST: /api/PokeFood
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult CreatePokeFood([FromBody] PokeFoodDtoCreate pokeFood)
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

            var pokeFoodMap = _mapper.Map<PokeFood>(pokeFood);

            if (!_pokeFoodRepository.CreatePokeFood(pokeFoodMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving the relationship.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetPokeFood),
                new { pokemonId = pokeFood.PokemonId, foodId = pokeFood.FoodId },
                pokeFoodMap);
        }

        // ✅ PUT: /api/PokeFood/{pokemonId}/{foodId}
        // Body: { "quantity": 5.0 }
        [HttpPut("{pokemonId:int}/{foodId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokeFood(int pokemonId, int foodId, [FromBody] PokeFoodDtoUpdate updatedPF)
        {
            if (updatedPF == null)
                return BadRequest("Invalid request body.");

            var existing = _pokeFoodRepository.GetPokeFood(pokemonId, foodId);
            if (existing == null)
                return NotFound($"Relationship with PokemonId={pokemonId} and FoodId={foodId} not found.");

            existing.Quantity = updatedPF.Quantity;

            if (!_pokeFoodRepository.UpdatePokeFood(existing))
                return StatusCode(500, "Something went wrong while updating the relationship.");

            return NoContent(); // 204 - success without body
        }

       
        [HttpDelete("{pokemonId:int}/{foodId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokeFood(int pokemonId, int foodId)
        {
            var pokeFood = _pokeFoodRepository.GetPokeFood(pokemonId, foodId);
            if (pokeFood == null)
                return NotFound("Relationship not found.");

            if (!_pokeFoodRepository.DeletePokeFood(pokeFood))
                return StatusCode(500, "Something went wrong deleting the relationship.");

            return NoContent();
        }
    }
}
