using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Food>))]
        //IEnumerable<Food> means a collection of Food objects
        public IActionResult GetFoods()
        {
            var foods = _mapper
                .Map<List<FoodDto>>(_foodRepository.GetFoods());
            //Mapping Food objects to FoodDto objects
            //Entity to DTO conversion

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(foods);
        }
        [HttpGet("foodId")]
        [ProducesResponseType(200, Type = typeof(Food))]
        [ProducesResponseType(400)]

        public IActionResult GetFood(int foodId)
        {
            if (!_foodRepository.FoodExists(foodId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var food = _mapper
                .Map<FoodDto>(_foodRepository.GetFood(foodId));
            return Ok(food);
        }
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateFood([FromBody] FoodDtoCreate foodCreate)
        {
            if (foodCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var food = _foodRepository.GetFoods()
            .Where(f => f.Name.Trim().ToUpper() == foodCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            //Trim(): removes all leading
            //and trailing white-space characters from the current string.
            //Toupper(): converts a string to uppercase.
            //TrimEnd(): removes all trailing white-space characters from the current string.

            //Her zaman düşün:
            //“Lambda içindeki harf(f, c, p...) bu tablodaki bir satırdır.
            //Onun alanlarına(.Id, .Name, .Type...) ulaşırım
            //ve onları benim elimdeki değişkenlerle karşılaştırırım.”

            if (food != null)
            {
                ModelState.AddModelError("", "Food already exists");
                return StatusCode(400, ModelState);
            }

            var foodMap = _mapper
                .Map<Food>(foodCreate);
            if (!_foodRepository.CreateFood(foodMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetFood), new { foodId = foodMap.Id }, foodMap);
            //CreatedAtAction: returns a 201 status code response
        }

        [HttpPut("foodId")]
        [ProducesResponseType(400)]
        [ProducesResponseTypeAttribute(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateFood(int foodId, [FromBody] FoodDto updatedFood)
        //foodId: which food to update. comes from URL
        //updatedFood: new data for the food. comes from HHTP BODY -json
        {
            if (updatedFood == null)
                return BadRequest(ModelState);

            if (foodId != updatedFood.Id)
                return BadRequest(ModelState);

            if (foodId != updatedFood.Id)
            {
                ModelState.AddModelError("", "This ID Duo is not matching");
                return BadRequest(ModelState);
            }
            if (!_foodRepository.FoodExists(foodId)) { 
            ModelState.AddModelError("", "This ID not found");
            return NotFound(ModelState);
        }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var foodMap = _mapper.Map<Food>(updatedFood);
            if (!_foodRepository.UpdateFood(foodMap))
            {
                ModelState.AddModelError("", "Something went wrong updating food");
                return StatusCode(500, ModelState);

                
            }
            if (_foodRepository.GetFoods().Any(f => f.Name.Trim().ToUpper() == 
            updatedFood.Name.Trim().ToUpper() && f.Id != foodId))
            {
                ModelState.AddModelError("", "A food with the same name already exists");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("foodId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public IActionResult DeleteFood(int foodId)
        {
            if (!_foodRepository.FoodExists(foodId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var foodToDelete = _foodRepository.GetFood(foodId);
            if (!_foodRepository.DeleteFood(foodToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting food");
                return StatusCode(500, ModelState);
            }
            else
            {
                return NoContent();
            }   
            
        }
        [HttpGet("by-pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FoodDto>))]
        [ProducesResponseType(404)]

        public IActionResult GetFoodsByPokemon(int pokeId)
        {
            var foods = _mapper.Map<List<FoodDto>>(
                _pokemonRepository.GetFoodsByPokemon(pokeId));
            if (foods == null || !foods.Any())
                return NotFound(new { message = $"No foods found for Pokemon {pokeId}" });

            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(foods);
        }
    }
}

