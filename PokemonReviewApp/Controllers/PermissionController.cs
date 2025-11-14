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
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(IPermissionRepository permissionRepository, IMapper mapper, ILogger<PermissionController> logger)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PermissionDto>))]
        public ActionResult<IEnumerable<PermissionDto>> GetPermissions()
        {
            var permissions = _permissionRepository.GetPermissions();
            var result = _mapper.Map<IEnumerable<PermissionDto>>(permissions);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PermissionDto))]
        [ProducesResponseType(404)]
        public ActionResult<PermissionDto> GetPermission(int id)
        {
            if (!_permissionRepository.PermissionExists(id))
                return NotFound();

            var permission = _permissionRepository.GetPermissionById(id);
            return Ok(_mapper.Map<PermissionDto>(permission));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<PermissionDto> CreatePermission([FromBody] PermissionCreateDto request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (_permissionRepository.GetPermissions().Any(p => p.PermissionName.ToLower() == request.PermissionName.ToLower()))
                return BadRequest("Permission already exists.");

            var permissionEntity = _mapper.Map<Permission>(request);
            if (!_permissionRepository.CreatePermission(permissionEntity))
            {
                _logger.LogError("Error creating permission: {PermissionName}", request.PermissionName);
                return StatusCode(500, "Something went wrong while saving.");
            }

            var response = _mapper.Map<PermissionDto>(permissionEntity);
            return CreatedAtAction(nameof(GetPermission), new { id = permissionEntity.Id }, response);
        }
    }
}
