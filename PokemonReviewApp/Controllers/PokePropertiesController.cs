//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;
//using PokemonReviewApp.Dto;
//using PokemonReviewApp.Interfaces;
//using PokemonReviewApp.Models;
//using System.Collections.Generic;


//namespace PokemonReviewApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PokePropertiesController : Controller
//    {
//        private readonly IPokePropertiesRepository _pokePropertiesRepository;
//        private readonly IPokemonRepository _pokemonRepository;
//        private readonly IPropertyRepository _propertyRepository;
//        private readonly IMapper _mapper;

//        public PokePropertiesController(IPokePropertiesRepository pokePropertiesRepository,
//        IPokemonRepository pokemonRepository,
//        IPropertyRepository propertyRepository,
//        IMapper mapper)
//        {
//            _pokePropertiesRepository = pokePropertiesRepository;
//            _pokemonRepository = pokemonRepository;
//            _propertyRepository = propertyRepository;
//            _mapper = mapper;
//        }

//        [HttpGet]
//        public IActionResult GetPokeProperties()
//        {
//            var pokeProperties = _mapper.Map<List<PokePropertyDto>>(_pokePropertiesRepository.GetPokeProperties());
//            return Ok(pokeProperties);
//        }
//        [HttpGet("{pokemonId}/{propertyId}")]
//        public IActionResult GetPokeProperty(int pokemonId, int propertyId)
//        {
//            var pokeProperty = _pokePropertiesRepository.GetPokeProperty(pokemonId, propertyId);
//            if (pokeProperty == null)
//                return NotFound();
//            return Ok(_mapper.Map<PokePropertyDto>(pokeProperty));
//        }

//        [HttpPost]
//        [ProducesResponseType(201)]
//        [ProducesResponseType(400)]
//        [ProducesResponseType(404)]
//        public IActionResult CreatePokeProperty(PokePropertyDtoCreate pokeProperty)
//        {
//            if (pokeProperty == null)
//                return BadRequest(ModelState);
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);
//            if (!_pokemonRepository.PokemonExists(pokeProperty.PokemonId))
//                return NotFound($"Pokemon with ID {pokeProperty.PokemonId} not found.");
//            if (!_propertyRepository.PropertyExists(pokeProperty.PropertyId))
//                return NotFound($"Property with ID {pokeProperty.PropertyId} not found.");
//            if (_pokePropertiesRepository.PokePropertyExists(pokeProperty.PokemonId, pokeProperty.PropertyId))
//                return BadRequest("This PokeProperty relationship already exists.");

//            var pokePropertyMap = _mapper
//                .Map<PokeProperty>(pokeProperty);

//            if (!_pokePropertiesRepository.CreatePokeProperty(pokePropertyMap))
//            {
//                return StatusCode(500, "Something went wrong while saving the PokeProperty.");

//            }
//            return Ok("Successfully created the PokeProperty.");
//        }
//        [HttpPut("{pokemonId}/{propertyId}")]
//        [ProducesResponseType(200)]
//        [ProducesResponseType(400)]
//        [ProducesResponseType(404)]
//        public IActionResult UpdatePokeProperty(int pokemonId, int propertyId, [FromBody] PokePropertyDtoUpdate updatedPP)
//        {
             
//        {
//            if (updatedPP == null)
//                return BadRequest("Invalid request body.");

//            var existing = _pokePropertiesRepository.GetPokeProperty(pokemonId, propertyId);
//            if (existing == null)
//                return NotFound("PokeProperty relationship not found.");

//            if (!_pokemonRepository.PokemonExists(updatedPP.ChangedPokemonId))
//                return NotFound($"Pokemon with ID {updatedPP.ChangedPokemonId} not found.");

//            if (!_propertyRepository.PropertyExists(updatedPP.ChangedPropertyId))
//                return NotFound($"Property with ID {updatedPP.ChangedPropertyId} not found.");

//            if (_pokePropertiesRepository.PokePropertyExists(updatedPP.ChangedPokemonId, updatedPP.ChangedPropertyId))
//                return BadRequest("This updated PokeProperty relationship already exists.");


//            _pokePropertiesRepository.DeletePokeProperty(existing);

//            var updated = new PokeProperty
//            {
//                PokemonId = updatedPP.ChangedPokemonId,
//                PropertyId = updatedPP.ChangedPropertyId
//            };

//            if (!_pokePropertiesRepository.CreatePokeProperty(updated))
//                return StatusCode(500, "Something went wrong while updating the PokeProperty.");

//            return Ok($"PokeProperty updated successfully from ({pokemonId}, {propertyId}) to ({updatedPP.ChangedPokemonId}, {updatedPP.ChangedPropertyId}).");
//        }
//        }
//        //{
//        //    if (updatedPP == null)
//        //        return BadRequest(ModelState);
//        //    var existing = _pokePropertiesRepository.GetPokeProperty(pokemonId, propertyId);
//        //    if (existing == null)
//        //        return NotFound("PokeProperty relationship not found.");
//        //    if (!ModelState.IsValid)
//        //        return BadRequest(ModelState);
//        //    if (!_pokemonRepository.PokemonExists(updatedPP.ChangedPokemonId))
//        //        return NotFound($"Pokemon with ID {updatedPP.ChangedPokemonId} not found.");
//        //    if (!_pokemonRepository.PokemonExists(updatedPP.ChangedPropertyId))
//        //        return NotFound($"Property with ID {updatedPP.ChangedPropertyId} not found.");
//        //    if (_pokePropertiesRepository.PokePropertyExists(updatedPP.ChangedPokemonId, updatedPP.ChangedPropertyId))
//        //        return BadRequest("This PokeProperty relationship already exists.");

//        //    _pokePropertiesRepository.DeletePokeProperty(existing);
//        //    var updated = new PokeProperty
//        //    {
//        //        PokemonId = updatedPP.ChangedPokemonId,
//        //        PropertyId = updatedPP.ChangedPropertyId
//        //    };
//        //    if(!_pokePropertiesRepository.CreatePokeProperty(updated))
//        //        return StatusCode(500, "Something went wrong updating the PokeProperty.");      

//        //    return Ok("PokeProperty updated successfully.");

//        //}
//        [HttpDelete("{pokemonId}/{propertyId}")]
//        public IActionResult DeletePokeProperty(int pokemonId, int propertyId)
//        {
//            var pokeProperty = _pokePropertiesRepository.GetPokeProperty(pokemonId, propertyId);
//            if (pokeProperty == null)
//                return NotFound("PokeProperty relationship not found.");
//            if (!_pokePropertiesRepository.DeletePokeProperty(pokeProperty))
//                return StatusCode(500, "Something went wrong deleting the PokeProperty.");
//            return Ok("PokeProperty deleted successfully.");

            
//        }

//    }
//}
