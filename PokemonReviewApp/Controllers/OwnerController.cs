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
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper
                .Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();

            var owner = _mapper
                .Map<OwnerDto>(_ownerRepository.GetOwner(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("{id}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
            {
                return NotFound();
            }

            var owner = _mapper.Map<List<PokemonDto>>(
                _ownerRepository.GetPokemonByOwner(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDtoCreate ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            // Aynı isimde Owner varsa hata
            var existingOwner = _ownerRepository.GetOwners()
                .FirstOrDefault(o => o.FirstName.Trim().ToUpper() == ownerCreate.FirstName.TrimEnd().ToUpper());

            if (existingOwner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            // Country kontrolü
            var country = _countryRepository.GetCountry(countryId);
            if (country == null)
            {
                ModelState.AddModelError("", "Invalid country id");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapping
            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = country; // Owner'ı o ülkeye bağla

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, "Successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateOwner(int id, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);

            if (id != updatedOwner.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ownerMap = _mapper.Map<Owner>(updatedOwner);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult SoftDeleteOwner(int id)
        {
            var owner = _ownerRepository.GetOwner(id);
            if (owner == null)
                return NotFound();

            _ownerRepository.SoftDeleteOwner(owner);
            _ownerRepository.Save();

            return NoContent();
        }
        [HttpPost("restore/{id}")]
        public IActionResult RestoreOwner(int id)
        {
            var owner = _ownerRepository.GetOwnerIncludingDeleted(id);

            if (owner == null)
                return NotFound("Owner not found");

            _ownerRepository.RestoreOwner(owner);
            _ownerRepository.Save();

            return Ok("Owner restored successfully");
        }
        [HttpGet("deleted")]
        public IActionResult GetDeletedOwners()
        {
            var deleted = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetDeletedOwners());
            return Ok(deleted);
        }

    }
}