using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helpers;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PokeFoodController : Controller
    {
        private readonly IPokeFoodRepository _pokeFoodRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public PokeFoodController(IPokeFoodRepository pokeFoodRepository,
                                  IPokemonRepository pokemonRepository,
                                  IFoodRepository foodRepository,
                                  IMapper mapper)
        {
            _pokeFoodRepository = pokeFoodRepository;
            _pokemonRepository = pokemonRepository;
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        // ---------------------------
        // GET ACTIVE LIST
        // ---------------------------
        [Authorize(Policy = "PokeFood.List")]
        [HttpGet]
        public IActionResult GetPokeFoods()
        {
            var list = _mapper.Map<List<PokeFoodDto>>(_pokeFoodRepository.GetPokeFoods());
            return Ok(list);
        }


        // ---------------------------
        // GET BY COMPOSITE KEY
        // ---------------------------
        [Authorize(Policy = "PokeFood.List")]
        [HttpGet("{pokemonId}/{foodId}")]
        public IActionResult GetPokeFood(int pokemonId, int foodId)
        {
            var entity = _pokeFoodRepository.GetPokeFood(pokemonId, foodId);
            if (entity == null) return NotFound();

            return Ok(_mapper.Map<PokeFoodDto>(entity));
        }


        // ---------------------------
        // CREATE
        // ---------------------------
        [Authorize(Policy = "PokeFood.Add")]
        [HttpPost]
        public IActionResult CreatePokeFood([FromBody] PokeFoodDtoCreate dto)
        {
            if (dto == null) return BadRequest("Invalid request.");

            if (!_pokemonRepository.PokemonExists(dto.PokemonId))
                return NotFound($"Pokemon {dto.PokemonId} not found.");

            if (!_foodRepository.FoodExists(dto.FoodId))
                return NotFound($"Food {dto.FoodId} not found.");

            // SOFT DELETED KAYIT VAR MI?
            var deleted = _pokeFoodRepository.GetPokeFoodIncludingDeleted(dto.PokemonId, dto.FoodId);

            if (deleted != null && deleted.IsDeleted)
            {
                // Restore → no audit
                _pokeFoodRepository.RestorePokeFood(dto.PokemonId, dto.FoodId);
                return Ok(new { message = "Restored successfully." });
            }

            // ACTIVE EXISTS?
            if (_pokeFoodRepository.PokeFoodExists(dto.PokemonId, dto.FoodId))
                return Conflict("Relationship already exists.");

            // CREATE
            var entity = _mapper.Map<PokeFood>(dto);
            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_pokeFoodRepository.CreatePokeFood(entity, userId))
                return StatusCode(500, "Error while saving.");

            return CreatedAtAction(nameof(GetPokeFood),
                new { pokemonId = dto.PokemonId, foodId = dto.FoodId },
                _mapper.Map<PokeFoodDto>(entity));
        }


        // ---------------------------
        // UPDATE
        // ---------------------------
        [Authorize(Policy = "PokeFood.Update")]
        [HttpPut("{pokemonId}/{foodId}")]
        public IActionResult UpdatePokeFood(int pokemonId, int foodId, [FromBody] PokeFoodDtoUpdate dto)
        {
            var entity = _pokeFoodRepository.GetPokeFood(pokemonId, foodId);
            if (entity == null)
                return NotFound("Relationship not found.");

            entity.Quantity = dto.Quantity;

            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_pokeFoodRepository.UpdatePokeFood(entity, userId))
                return StatusCode(500, "Error updating.");

            return NoContent();
        }


        // ---------------------------
        // SOFT DELETE
        // ---------------------------
        [Authorize(Policy = "PokeFood.Delete")]
        [HttpDelete("{pokemonId}/{foodId}")]
        public IActionResult SoftDeletePokeFood(int pokemonId, int foodId)
        {
            var entity = _pokeFoodRepository.GetPokeFoodIncludingDeleted(pokemonId, foodId);
            if (entity == null) return NotFound();

            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_pokeFoodRepository.SoftDeletePokeFood(pokemonId, foodId, userId))
                return StatusCode(500, "Delete failed.");

            return NoContent();
        }


        // ---------------------------
        // RESTORE
        // ---------------------------
        [Authorize(Policy = "PokeFood.Restore")]
        [HttpPost("restore/{pokemonId}/{foodId}")]
        public IActionResult RestorePokeFood(int pokemonId, int foodId)
        {
            var entity = _pokeFoodRepository.GetPokeFoodIncludingDeleted(pokemonId, foodId);
            if (entity == null) return NotFound();

            if (!entity.IsDeleted)
                return BadRequest("Already active.");


            if (!_pokeFoodRepository.RestorePokeFood(pokemonId, foodId))
                return StatusCode(500, "Restore failed.");

            return Ok("Restored successfully.");
        }


        // ---------------------------
        // DELETED LIST
        // ---------------------------
        [Authorize(Policy = "PokeFood.ListDeleted")]
        [HttpGet("deleted")]
        public IActionResult GetDeletedPokeFoods()
        {
            var deleted = _mapper.Map<List<PokeFoodDto>>(_pokeFoodRepository.GetDeletedPokeFoods());
            return Ok(deleted);
        }
    }
}
