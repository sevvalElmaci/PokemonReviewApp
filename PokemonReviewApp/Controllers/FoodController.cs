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
    public class FoodController : Controller
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;

        public FoodController(IPokemonRepository pokemonRepository, IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
        }


        [Authorize(Policy = "Food.List")]
        [HttpGet]
        public IActionResult GetFoods()
        {
            var foods = _mapper.Map<List<FoodDto>>(_foodRepository.GetFoods());
            return Ok(foods);
        }

        [Authorize(Policy = "Food.List")]
        [HttpGet("{id}")]
        public IActionResult GetFood(int id)
        {
            if (!_foodRepository.FoodExists(id))
                return NotFound();

            var food = _mapper.Map<FoodDto>(_foodRepository.GetFood(id));
            return Ok(food);
        }

        [Authorize(Policy = "Food.List")]
        [HttpGet("by-pokemon/{pokeId}")]
        public IActionResult GetFoodsByPokemon(int pokeId)
        {
            var foods = _mapper.Map<List<FoodDto>>(_pokemonRepository.GetFoodsByPokemon(pokeId));

            if (foods == null || !foods.Any())
                return NotFound(new { message = $"No foods found for Pokemon {pokeId}" });

            return Ok(foods);
        }


        [Authorize(Policy = "Food.Add")]
        [HttpPost]
        public IActionResult CreateFood([FromBody] FoodDtoCreate foodCreate)
        {
            if (foodCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var allFoods = _foodRepository.GetFoodsIncludingDeleted();

            var activeFood = allFoods.FirstOrDefault(f =>
                f.Name.Trim().ToUpper() == foodCreate.Name.Trim().ToUpper() &&
                f.IsDeleted == false);

            var deletedFood = allFoods.FirstOrDefault(f =>
                f.Name.Trim().ToUpper() == foodCreate.Name.Trim().ToUpper() &&
                f.IsDeleted == true);

            // Aktif duplicate
            if (activeFood != null)
                return Conflict("Food already exists");

            // Silinmiş duplicate
            if (deletedFood != null)
            {
                if (User.IsInRole("Admin"))
                    return Conflict("A deleted food with this name exists. You can restore it.");

                _foodRepository.RestoreFood(deletedFood);
                _foodRepository.Save();

                return Ok(new
                {
                    message = "Food restored successfully",
                    food = _mapper.Map<FoodDto>(deletedFood)
                });
            }

            // Yeni kayıt
            var foodMap = _mapper.Map<Food>(foodCreate);

            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_foodRepository.CreateFood(foodMap, userId))
                return StatusCode(500, "Something went wrong while saving");

            return CreatedAtAction(nameof(GetFood), new { id = foodMap.Id }, foodMap);
        }



        [Authorize(Policy = "Food.Update")]
        [HttpPut("{id}")]
        public IActionResult UpdateFood(int id, [FromBody] FoodDto updatedFood)
        {
            if (updatedFood == null || updatedFood.Id != id)
                return BadRequest();

            if (!_foodRepository.FoodExists(id))
                return NotFound();

            bool duplicateExists = DuplicateCheckHelper.ExistsDuplicate(
                _foodRepository.GetFoods(),
                f => f.Name,
                updatedFood.Name,
                id,
                f => f.Id
            );

            if (duplicateExists)
                return Conflict("Another food with this name already exists");

            var existingFood = _foodRepository.GetFood(id);
            existingFood.Name = updatedFood.Name;

            int userId = int.Parse(User.FindFirst("userId").Value);    

            if (!_foodRepository.UpdateFood(existingFood, userId))
                return StatusCode(500, "Something went wrong while updating");

            return NoContent();
        }

        [Authorize(Policy = "Food.Delete")]
        [HttpDelete("{id}")]
        public IActionResult SoftDeleteFood(int id)
        {
            var food = _foodRepository.GetFood(id);
            if (food == null)
                return NotFound();

            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_foodRepository.SoftDelete(id, userId))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }



        [Authorize(Policy = "Food.Restore")]
        [HttpPost("restore/{id}")]
        public IActionResult RestoreFood(int id)
        {
            var food = _foodRepository.GetFoodIncludingDeleted(id);
            if (food == null)
                return NotFound("Food not found");

            var duplicateActive = _foodRepository.GetFoodsIncludingDeleted()
                .Any(f => f.Name.Trim().ToUpper() == food.Name.Trim().ToUpper()
                       && f.IsDeleted == false
                       && f.Id != id);

            if (duplicateActive)
            {
                ModelState.AddModelError("",
                    "Cannot restore. There is already an active food with the same name.");
                return Conflict(ModelState);
            }

            _foodRepository.RestoreFood(food);
            _foodRepository.Save();

            return Ok("Food restored successfully!");
        }



        [Authorize(Policy = "Food.ListDeleted")]
        [HttpGet("deleted")]
        public IActionResult GetDeletedFoods()
        {
            var deletedFoods = _mapper.Map<List<FoodDto>>(_foodRepository.GetDeletedFoods());
            return Ok(deletedFoods);
        }
    }
}
