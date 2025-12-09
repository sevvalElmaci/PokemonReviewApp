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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        // LIST
        [Authorize(Policy = "Owner.List")]
        [HttpGet]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
            return Ok(owners);
        }

        // GET BY ID
        [Authorize(Policy = "Owner.List")]
        [HttpGet("{id}")]
        public IActionResult GetOwner(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(id));
            return Ok(owner);
        }

        // GET POKEMONS OF OWNER
        [Authorize(Policy = "Owner.List")]
        [HttpGet("{id}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        public IActionResult GetPokemonByOwner(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(
                _ownerRepository.GetPokemonByOwner(id));

            return Ok(pokemons);
        }

        [Authorize(Policy = "Owner.Add")]
        [HttpPost]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDtoCreate ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            if (countryId <= 0)
                return BadRequest("Please add a country ID");

            var allOwners = _ownerRepository.GetOwners()
                .Concat(_ownerRepository.GetDeletedOwners())
                .ToList();

            var fn = ownerCreate.FirstName.Trim().ToUpper();
            var ln = ownerCreate.LastName.Trim().ToUpper();

            var activeOwner = allOwners.FirstOrDefault(o =>
                o.FirstName.ToUpper() == fn &&
                o.LastName.ToUpper() == ln &&
                o.Country != null &&
                o.Country.Id == countryId &&
                !o.IsDeleted);

            var deletedOwner = allOwners.FirstOrDefault(o =>
                o.FirstName.ToUpper() == fn &&
                o.LastName.ToUpper() == ln &&
                o.Country != null &&
                o.Country.Id == countryId &&
                o.IsDeleted);

            // aktif duplicate
            if (activeOwner != null)
                return Conflict("Owner already exists.");

            // silinmiş duplicate → auto restore (Admin hariç)
            if (deletedOwner != null)
            {
                if (User.IsInRole("Admin"))
                    return Conflict("A deleted owner exists. Please restore it from deleted list.");

                int userId = int.Parse(User.FindFirst("userId").Value);

                _ownerRepository.Save();

                return Ok(new
                {
                    message = "Owner restored successfully",
                    owner = _mapper.Map<OwnerDto>(deletedOwner)
                });
            }

            var country = _countryRepository.GetCountry(countryId);
            if (country == null)
                return BadRequest("Invalid country ID");

            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = country;

            int createUserId = int.Parse(User.FindFirst("userId").Value);

            if (!_ownerRepository.CreateOwner(ownerMap, createUserId))
                return StatusCode(500, "Something went wrong while saving");

            return StatusCode(201, "Successfully created");
        }



        [Authorize(Policy = "Owner.Update")]
        [HttpPut("{id}")]
        public IActionResult UpdateOwner(int id, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null || updatedOwner.Id != id)
                return BadRequest();

            var owner = _ownerRepository.GetOwnerIncludingDeleted(id);
            if (owner == null)
                return NotFound();  

            owner.FirstName = updatedOwner.FirstName;
            owner.LastName = updatedOwner.LastName;
            owner.Gym = updatedOwner.Gym;

            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_ownerRepository.UpdateOwner(owner, userId))
                return StatusCode(500, "Something went wrong while updating");

            return NoContent();
        }

        //SOFTDELETE
        [Authorize(Policy = "Owner.Delete")]
        [HttpDelete("{id}")]
        public IActionResult SoftDeleteOwner(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();

            int userId = int.Parse(User.FindFirst("userId").Value);

            if (!_ownerRepository.SoftDeleteOwner(id, userId))
                return StatusCode(500, "Something went wrong while deleting");

            return NoContent();
        }


        //Restore
        [Authorize(Policy = "Owner.Restore")]
        [HttpPost("restore/{id}")]
        public IActionResult RestoreOwner(int id)
        {
            var owner = _ownerRepository.GetOwnerIncludingDeleted(id);
            if (owner == null)
                return NotFound("Owner not found");

            var duplicateActive = _ownerRepository.GetOwners()
    .Any(o =>
        o.FirstName.Trim().ToUpper() == owner.FirstName.Trim().ToUpper() &&
        o.LastName.Trim().ToUpper() == owner.LastName.Trim().ToUpper() &&
        o.Country != null &&
        owner.Country != null &&
        o.Country.Id == owner.Country.Id &&
        o.IsDeleted == false &&
        o.Id != id
    );

            if (duplicateActive)
                return Conflict("An active owner with the same name in this country already exists.");


            if (duplicateActive)
            {
                ModelState.AddModelError("",
                    "Cannot restore. There is already an active owner with the same name in this country.");
                return Conflict(ModelState);
            }

            _ownerRepository.RestoreOwner(owner);
            _ownerRepository.Save();

            return Ok("Owner restored successfully");
        }


        // LIST DELETED
        [Authorize(Policy = "Owner.ListDeleted")]
        [HttpGet("deleted")]
        public IActionResult GetDeletedOwners()
        {
            var deleted = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetDeletedOwners());
            return Ok(deleted);
        }
    }
}
