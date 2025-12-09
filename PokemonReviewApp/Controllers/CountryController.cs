using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helpers;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [Authorize(Policy = "Country.List")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [Authorize(Policy = "Country.ListDeleted")]
        [HttpGet("deleted")]
        [ProducesResponseType(200)]
        public IActionResult GetDeletedCountries()
        {
            var deletedCountries = _mapper.Map<List<CountryDto>>(_countryRepository.GetDeletedCountries());
            return Ok(deletedCountries);
        }

        [Authorize(Policy = "Country.List")]
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [Authorize(Policy = "Country.Restore")]
        [HttpPost("restore/{id}")]
        public IActionResult RestoreCountry(int id)
        {
            var country = _countryRepository.GetCountryIncludingDeleted(id);
            if (country == null)
                return NotFound("Country not found");

            // Duplicate Active Check
            var duplicateActive = _countryRepository.GetCountriesIncludingDeleted()
                .Any(c => c.Name.Trim().ToUpper() == country.Name.Trim().ToUpper()
                       && c.IsDeleted == false
                       && c.Id != id);

            if (duplicateActive)
            {
                ModelState.AddModelError("",
                    "Cannot restore. There is already an active country with the same name.");
                return Conflict(ModelState);
            }

            _countryRepository.RestoreCountry(country);
            _countryRepository.Save();

            return Ok("Country restored successfully!");
        }


        [Authorize(Policy = "Country.Add")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]

        public IActionResult CreateCountry([FromBody] CountryDtoCreate countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var allCountries = _countryRepository.GetCountriesIncludingDeleted();
            var userId = int.Parse(User.FindFirst("userId").Value);

            var activeCountry = allCountries
                .FirstOrDefault(c =>
                    c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper()
                    && !c.IsDeleted);

            if (activeCountry != null)
                return Conflict("Country already exists");

            var deletedCountry = allCountries
                .FirstOrDefault(c =>
                    c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper()
                    && c.IsDeleted);

            if (deletedCountry != null)
            {
                deletedCountry.IsDeleted = false;
                deletedCountry.DeletedDateTime = null;
                deletedCountry.DeletedUserId = null;

                deletedCountry.UpdatedUserId = userId;
                deletedCountry.UpdatedDateTime = DateTime.Now;

                _countryRepository.UpdateCountry(deletedCountry, userId);
                return Ok("Country restored from deleted list.");
            }

            var newCountry = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(newCountry, userId))
                return StatusCode(500, "Something went wrong while saving");

            return CreatedAtAction(nameof(GetCountry),
                new { countryId = newCountry.Id },
                _mapper.Map<Country>(countryCreate));
        }


        [Authorize(Policy = "Country.Update")]
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var allCountries = _countryRepository.GetCountries();

            bool duplicateExists = DuplicateCheckHelper.ExistsDuplicate(
                _countryRepository.GetCountries(),
                c => c.Name,
                updatedCountry.Name,
                countryId,
                c => c.Id
            );

            if (duplicateExists)
            {   
                return Conflict("A country with the same name already exists");
            }

            var existingCountry = _countryRepository.GetCountry(countryId);
            if (existingCountry == null)
                return NotFound();

            existingCountry.Name = updatedCountry.Name;

            var userId = int.Parse(User.FindFirst("userId").Value);


            if (!_countryRepository.UpdateCountry(existingCountry, userId))
            {
                return StatusCode(500, "Something went wrong while updating");
            }

            return NoContent();
        }

        [Authorize(Policy = "Country.Delete")]
        [HttpDelete("{id}")]
        public IActionResult SoftDeleteCountry(int id)
        {
            var country = _countryRepository.GetCountry(id);
            if (country == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst("userId").Value);


            if (!_countryRepository.SoftDeleteCountry(id, userId))
                return StatusCode(500, "Failed to delete country");

            return NoContent();
        }
    }
}
