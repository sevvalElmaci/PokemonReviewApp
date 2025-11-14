using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Dto;
using System.Net.Mime;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleRepository roleRepository, IMapper mapper, ILogger<RoleController> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoleDto>))]
        public ActionResult<IEnumerable<RoleDto>> GetRoles()
        {
            var roles = _roleRepository.GetRoles();
            var result = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(RoleDto))]
        [ProducesResponseType(404)]
        public ActionResult<RoleDto> GetRole(int id)
        {
            if (!_roleRepository.RoleExists(id))
                return NotFound();

            var role = _roleRepository.GetRoleById(id);
            return Ok(_mapper.Map<RoleDto>(role));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<RoleDto> CreateRole([FromBody] RoleCreateDto request)
        {
            if (request == null)
                return BadRequest();

            if (_roleRepository.GetRoles().Any(r => r.RoleName.ToLower() == request.RoleName.ToLower()))
                return BadRequest("Role already exists.");

            var roleEntity = _mapper.Map<Role>(request);
            if (!_roleRepository.CreateRole(roleEntity))
            {
                _logger.LogError("Error creating role: {RoleName}", request.RoleName);
                return StatusCode(500, "Something went wrong while saving.");
            }

            var response = _mapper.Map<RoleDto>(roleEntity);
            return CreatedAtAction(nameof(GetRole), new { id = roleEntity.Id }, response);
        }
    }
}
