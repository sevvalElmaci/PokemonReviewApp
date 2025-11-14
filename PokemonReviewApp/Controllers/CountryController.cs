using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helpers;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;


        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);

        }
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
        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(
                _countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDtoCreate countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);

            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            if ((!_countryRepository.CreateCountry(countryMap)))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

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
            //if country exist you can do update operation

            if (!ModelState.IsValid)
                return BadRequest();
            // are data correct or not based on model state rules and protocols
            //control line yaniii

            bool duplicateExists = DuplicateCheckHelper.ExistsDuplicate(
    _countryRepository.GetCountries(),
    c => c.Name,
    updatedCountry.Name,
    countryId,
    c => c.Id
);
            if (duplicateExists)
            {
                ModelState.AddModelError("", "A country with the same name already exists");
                return Conflict(ModelState);
            }

            //  Mevcut entity’yi getirip alanlarını güncelle
            var existingCountry = _countryRepository.GetCountry(countryId);
            if (existingCountry == null)
                return NotFound();

            existingCountry.Name = updatedCountry.Name;

            // EF zaten tracked olduğu için yeni nesne oluşturma
            if (!_countryRepository.UpdateCountry(existingCountry))
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


        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]

        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }


}


