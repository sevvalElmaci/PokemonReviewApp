using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helpers;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
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
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Food))]
        [ProducesResponseType(400)]

        public IActionResult GetFood(int id)
        {
            if (!_foodRepository.FoodExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var food = _mapper
                .Map<FoodDto>(_foodRepository.GetFood(id));
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

            return CreatedAtAction(nameof(GetFood), new { id = foodMap.Id }, foodMap);
            //CreatedAtAction: returns a 201 status code response
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseTypeAttribute(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateFood(int id, [FromBody] FoodDto updatedFood)
        //id: which food to update. comes from URL
        //updatedFood: new data for the food. comes from HHTP BODY -json
        {
            if (updatedFood == null)
                return BadRequest(ModelState);

            if (id != updatedFood.Id)
                return BadRequest(ModelState);

            if (id != updatedFood.Id)
            {
                ModelState.AddModelError("", "This ID Duo is not matching");
                return BadRequest(ModelState);
            }
            if (!_foodRepository.FoodExists(id)) { 
            ModelState.AddModelError("", "This ID not found");
            return NotFound(ModelState);
        }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool duplicateExists = DuplicateCheckHelper.ExistsDuplicate(
    _foodRepository.GetFoods(),
    f => f.Name,
    updatedFood.Name,
    id,
    f => f.Id
);
            if (duplicateExists)
            {
                ModelState.AddModelError("", "A food with the same name already exists");
                return Conflict(ModelState);
            }


            //  Mevcut entity’yi getirip alanlarını güncelle
            var existingFood = _foodRepository.GetFood(id);
            if (existingFood == null)
                return NotFound();

            existingFood.Name = updatedFood.Name;

            // EF zaten tracked olduğu için yeni nesne oluşturma
            if (!_foodRepository.UpdateFood(existingFood))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
            //Put method does not return any content after updating 
            //cause you didnt create anything new
            // you just updated existing one
            //mappleme : DTO -> entity

            //update için mappleme yapmıyoruz, çünkü EF'in izlediği (tracked ettiği) bir entity var zaten
            //DTO'dan yeni bir entity oluşturursan Ef der ki: BEN HANGİSİNİ İZLİYİM??
            //update için mevcut entity'i manuel güncellemek mantıklıdır.
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public IActionResult SoftDeleteFood(int id)
        {
            var food = _foodRepository.GetFood(id);
            if (food == null)
                return NotFound();

            _foodRepository.SoftDelete(food);
            _foodRepository.Save();

            return NoContent();
        }

        [HttpPost("restore/{id}")]
        public IActionResult RestoreFood(int id)
        {
            var food = _foodRepository.GetFoodIncludingDeleted(id);
            if (food == null)
                return NotFound("Food not found");

            _foodRepository.RestoreFood(food);
            _foodRepository.Save();

            return Ok("Food restored successfully");
        }
        [HttpGet("deleted")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FoodDto>))]
        public IActionResult GetDeletedFoods()
        {
            var deletedFoods = _mapper.Map<List<FoodDto>>(_foodRepository.GetDeletedFoods());
            return Ok(deletedFoods);
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

