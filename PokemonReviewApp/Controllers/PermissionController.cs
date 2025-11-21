using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionController(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        // GET: api/permission
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PermissionDto>))]
        public IActionResult GetPermissions()
        {
            var permissions = _permissionRepository.GetPermissions();
            var result = _mapper.Map<List<PermissionDto>>(permissions);
            return Ok(result);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDeletedPermissions()
        {
            var deletedPermissions = _permissionRepository.GetDeletedPermissions();
            var result = _mapper.Map<IEnumerable<PermissionDto>>(deletedPermissions);
            return Ok(result);
        }


        // GET: api/permission/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PermissionDto))]
        [ProducesResponseType(404)]
        public IActionResult GetPermission(int id)
        {
            if (!_permissionRepository.PermissionExists(id))
                return NotFound("Permission not found");

            var permission = _permissionRepository.GetPermissionById(id);
            var result = _mapper.Map<PermissionDto>(permission);

            return Ok(result);
        }

        // POST: api/permission
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreatePermission([FromBody] PermissionCreateDto request)
        {
            if (request == null)
                return BadRequest(ModelState);

            if (_permissionRepository.PermissionExists(request.PermissionName))
            {
                ModelState.AddModelError("", "Permission already exists");
                return Conflict(ModelState);
            }

            var permissionEntity = _mapper.Map<Permission>(request);

            if (!_permissionRepository.CreatePermission(permissionEntity))
            {
                ModelState.AddModelError("", "Something went wrong while saving permission");
                return StatusCode(500, ModelState);
            }

            return Ok("Permission created");
        }

        // PUT: api/permission/5
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePermission(int id, [FromBody] PermissionCreateDto request)
        {
            if (request == null)
                return BadRequest(ModelState);

            var permission = _permissionRepository.GetPermissionById(id);
            if (permission == null)
                return NotFound("Permission not found");

            // duplicate name kontrolü
            if (_permissionRepository.GetPermissions()
                .Any(p => p.PermissionName.ToLower() == request.PermissionName.ToLower() && p.Id != id))
            {
                ModelState.AddModelError("", "A permission with the same name already exists");
                return Conflict(ModelState);
            }

            // --- 🔥 Audit + Update işlemi ---
            permission.PermissionName = request.PermissionName;
            permission.UpdatedDateTime = DateTime.Now;
            permission.UpdatedUserId = 1;  // login sonrası JWT'den alacağız

            if (!_permissionRepository.UpdatePermission(permission))
            {
                ModelState.AddModelError("", "Something went wrong while updating permission");
                return StatusCode(500, ModelState);
            }

            return Ok("Permission updated");
        }


        // DELETE: api/permission/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult SoftDeletePermission(int id)
        {
            var permission = _permissionRepository.GetPermissionIncludingDeleted(id);

            if (permission == null)
                return NotFound("Permission not found");

            if (!_permissionRepository.SoftDeletePermission(permission))
                return StatusCode(500, "Error deleting permission");

            return Ok("Permission deleted");
        }
        [HttpPost("restore/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RestorePermission(int id)
        {
            var permission = _permissionRepository.GetPermissionIncludingDeleted(id);

            if (permission == null)
                return NotFound("Permission not found");

            if (!permission.IsDeleted)
                return BadRequest("Permission is not deleted");

            if (!_permissionRepository.RestorePermission(permission))
            {
                ModelState.AddModelError("", "Something went wrong while restoring permission");
                return StatusCode(500, ModelState);
            }

            return Ok("Permission restored successfully");
        }


    }
}
