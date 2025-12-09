using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(
            IPokemonRepository pokemonRepository,
            IMapper mapper,
            IOwnerRepository ownerRepository,
            ICategoryRepository categoryRepository,
            IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
        }

        // ============================
        // LIST
        // ============================
        [Authorize(Policy = "Pokemon.List")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        // ============================
        // GET BY ID
        // ============================
        [Authorize(Policy = "Pokemon.List")]
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(PokemonDto))]
        [ProducesResponseType(404)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        // ============================
        // RATING
        // ============================
        [Authorize(Policy = "Pokemon.List")]
        [HttpGet("{pokeId}/rating")]
        // ⭐ CHANGE: Type artık Pokemon değil, rating (double) döndürüyoruz
        [ProducesResponseType(200, Type = typeof(double))]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            if (!_reviewRepository.GetReviewsForAPokemon(pokeId).Any())
                return NotFound("No reviews found for this pokemon");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            return Ok(rating);
        }

        [Authorize(Policy = "Pokemon.Add")]
        [HttpPost]
        public IActionResult CreatePokemon(
    [FromQuery] int ownerId,
    [FromQuery] int catId,
    [FromQuery] int foodId,
    [FromBody] PokemonDtoCreate pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //// 🔍 Get all incl. deleted
            //var allPokemons = _pokemonRepository.GetOwnerIncludingDeleted();

            //var activePokemon = allPokemons.FirstOrDefault(p =>
            //    p.Name.Trim().ToUpper() == pokemonCreate.Name.Trim().ToUpper() &&
            //    p.IsDeleted == false);

            //var deletedPokemon = allPokemons.FirstOrDefault(p =>
            //    p.Name.Trim().ToUpper() == pokemonCreate.Name.Trim().ToUpper() &&
            //    p.IsDeleted == true);

            //// 1️⃣ Active duplicate
            //if (activePokemon != null)
            //{
            //    ModelState.AddModelError("", "Pokemon already exists");
            //    return Conflict(ModelState);
            //}

            //// 2️⃣ Deleted duplicate
            //if (deletedPokemon != null)
            //{
            //    if (User.IsInRole("Admin"))
            //    {
            //        ModelState.AddModelError("",
            //            "A deleted pokemon with this name exists. You can restore it from deleted list.");
            //        return Conflict(ModelState);
            //    }

            //    _pokemonRepository.RestorePokemon(deletedPokemon);
            //    _pokemonRepository.Save();

            //    return Ok(new
            //    {
            //        message = "Pokemon restored successfully",
            //        pokemon = _mapper.Map<PokemonDto>(deletedPokemon)
            //    });
            //}

            //// 3️⃣ Brand new
            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            var userId = int.Parse(User.FindFirst("userId").Value);

            var created = _pokemonRepository.CreatePokemon(ownerId, catId, foodId, pokemonMap, userId);
            

            if (!created)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            var dto = _mapper.Map<PokemonDto>(pokemonMap);

            return CreatedAtAction(nameof(GetPokemon),
                new { pokeId = pokemonMap.Id },
                dto);
        }


        // ============================
        // UPDATE
        // ============================
        [Authorize(Policy = "Pokemon.Update")]
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

            // ⭐ CHANGE: Burada artık komple yeni entity map'lemiyoruz.
            // DB'den mevcut Pokemon'u çekip sadece gerekli alanları güncelliyoruz.
            var existingPokemon = _pokemonRepository.GetPokemon(pokeId);
            if (existingPokemon == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst("userId").Value);

            existingPokemon.Name = updatedPokemon.Name;
            existingPokemon.BirthDate = updatedPokemon.BirthDate;

            existingPokemon.UpdatedUserId = userId;
            existingPokemon.UpdatedDateTime = DateTime.Now;

            if (!_pokemonRepository.UpdatePokemon(existingPokemon, userId))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
        // ============================
        // SOFT DELETE
        // ============================
        [Authorize(Policy = "Pokemon.Delete")]
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult SoftDeletePokemon(int pokeId)
        {
            var pokemon = _pokemonRepository.GetPokemon(pokeId);
            if (pokemon == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst("userId").Value);

            pokemon.DeletedUserId = userId;
            pokemon.DeletedDateTime = DateTime.Now;

            _pokemonRepository.SoftDeletePokemon(pokemon, userId);
            _pokemonRepository.Save();

            return NoContent();
      
        }

        [Authorize(Policy = "Pokemon.Restore")]
        [HttpPost("restore/{id}")]
        public IActionResult RestorePokemon(int id)
        {
            var pokemon = _pokemonRepository.GetPokemonIncludingDeleted(id);
            if (pokemon == null)
                return NotFound("Pokemon not found");

            //var duplicateActive = _pokemonRepository.GetPokemons()
            //    .Any(p =>
            //        p.Name.Trim().ToUpper() == pokemon.Name.Trim().ToUpper() &&
            //        p.IsDeleted == false &&
            //        p.Id != id);

            //if (duplicateActive)
            //{
            //    ModelState.AddModelError("",
            //        "Cannot restore. There is already an active pokemon with the same name.");
            //    return Conflict(ModelState);
            //}

            _pokemonRepository.RestorePokemon(pokemon);
            _pokemonRepository.Save();

            return Ok("Pokemon restored successfully");
        }


        // ============================
        // LIST DELETED
        // ============================
        [Authorize(Policy = "Pokemon.ListDeleted")]
        [HttpGet("deleted")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        public IActionResult GetDeletedPokemons()
        {
            var deleted = _mapper.Map<List<PokemonDto>>(
                _pokemonRepository.GetDeletedPokemons());

            return Ok(deleted);
        }

        //// ============================
        //// SPECIAL ADMIN ACTION
        //// ============================
        //[Authorize(Roles = "Admin")]
        //[HttpPost("create-with-log")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        //public IActionResult CreatePokemonWithLog(
        //    [FromQuery] int ownerId, 
        //    [FromQuery]  int categoryId, 
        //    [FromQuery] int foodId,
        //    [FromBody] PokemonDto pokemonDto)
        //{
        //    if (pokemonDto == null)
        //        return BadRequest(ModelState);

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var userId = int.Parse(User.FindFirst("userId").Value);


        //    var pokemon = _mapper.Map<Pokemon>(pokemonDto);

        //    var result = _pokemonRepository.CreatePokemonWithLog(
        //        ownerId,
        //        categoryId, 
        //        foodId,
        //        pokemon,
        //        userId);


        //    if (!result)
        //        ModelState.AddModelError("", "Something went wrong while saving");


        //    var dto = _mapper.Map<PokemonDto>(pokemon);

        //    return Ok(dto);
        //}
    }
}
