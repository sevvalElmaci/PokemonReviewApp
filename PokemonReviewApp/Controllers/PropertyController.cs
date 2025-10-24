using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using Deneme = PokemonReviewApp.Models;
namespace PokemonReviewApp.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        public PropertyController(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        //this endpoint is to get all categories. so we dont need any parameter. it just returns all categories
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Property>))]
        public IActionResult GetProperties()
        {
            var properties = _mapper.Map<List<PropertyDto>>(_propertyRepository.GetProperties());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(properties);
        }

        //this endpoint is to get a specific category by its id. so we need to pass the categoryId as parameter
        // GET: api/property/5
        [HttpGet("{propertyId}")]
        [ProducesResponseType(200, Type = typeof(Property))]
        [ProducesResponseType(404)]
        public IActionResult GetProperty(int propertyId)
        {
            if (!_propertyRepository.PropertyExists(propertyId))
                return NotFound();

            var property = _mapper.Map<PropertyDto>(_propertyRepository.GetProperty(propertyId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(property);
        }
        //this endpoint is to create a new category.
        ///so we need to pass the category data in the request body 
        ///cause you have to define the new category details in body part with JSON 
        // POST: api/property
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateProperty([FromBody] PropertyDtoUpdate propertyCreate)
        {
            if (propertyCreate == null)
                return BadRequest(ModelState);

            var property = _propertyRepository.GetProperties()
                .FirstOrDefault(p => p.Name.Trim().ToUpper() == propertyCreate.Name.TrimEnd().ToUpper());
            if (property != null)
            {
                ModelState.AddModelError("", "Property already exists");
                return StatusCode(400, ModelState);
            }

            var propertyMap = _mapper.Map<Property>(propertyCreate);
            if (!_propertyRepository.CreateProperty(propertyMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetProperty), new { propertyId = propertyMap.Id }, propertyMap);
        }

        //this endpoint is to update an existing category. 
        //so we need 2 different parameter. first one is answer to "which data will update" 
        //second one is answer to "how data will update, give me details in body part"

        // PUT: api/property/5
        [HttpPut("{propertyId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        
        public IActionResult UpdateProperty(int propertyId, [FromBody] PropertyDtoUpdate updatedProperty)
        {
            if (updatedProperty == null)
                return BadRequest("Invalid data.");

            // 1️⃣ Mevcut property var mı kontrol et
            var existing = _propertyRepository.GetProperty(propertyId);
            if (existing == null)
                return NotFound("Property not found.");

            // 2️⃣ Aynı isimde ve id’si farklı başka bir property var mı kontrol et
            var duplicate = _propertyRepository
                .GetProperties()
                .FirstOrDefault(p =>
                    p.Name.Trim().ToLower() == updatedProperty.Name.Trim().ToLower() &&
                    p.Id != propertyId);

            if (duplicate != null)
                return BadRequest("A property with the same name already exists.");

            // 3️⃣ Mevcut entity'yi güncelle (tracked durumda)
            _mapper.Map(updatedProperty, existing);

            if (!_propertyRepository.Save())
                return StatusCode(500, "Error updating property");

            return NoContent();
        }

        //this endpoint is to delete an existing category.
        // DELETE: api/property/5
        [HttpDelete("{propertyId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProperty(int propertyId)
        {
            if (!_propertyRepository.PropertyExists(propertyId))
                return NotFound();

            var propertyToDelete = _propertyRepository.GetProperty(propertyId);
            if (!_propertyRepository.DeleteProperty(propertyToDelete))
            {
                ModelState.AddModelError("", "Error deleting property");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //[HttpGet]
        ////this endpoint is to check if a category exists or not.
        //public IActionResult PropertyExists(int id)
        //{
        //    bool exists = _propertyRepository.PropertyExists(id);
        //    return Ok(exists);
        //}
        //[HttpGet]
        //public IActionResult Save()
        //{
        //    bool saved = _propertyRepository.Save();
        //    return Ok(saved);
        //}
    }
}