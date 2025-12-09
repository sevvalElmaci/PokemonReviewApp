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
    [Authorize(Roles = "Admin")]   // Only Admin can manage roles
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirst("userId").Value);

        // ============================================
        // GET ALL ROLES
        // ============================================
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetRoles()
        {
            var roles = _roleRepository.GetRoles();
            var dto = _mapper.Map<List<RoleDto>>(roles);
            return Ok(dto);
        }

        // ============================================
        // GET DELETED ROLES
        // ============================================
        [HttpGet("deleted")]
        [ProducesResponseType(200)]
        public IActionResult GetDeletedRoles()
        {
            var roles = _roleRepository.GetDeletedRoles();
            var dto = _mapper.Map<List<RoleDto>>(roles);
            return Ok(dto);
        }

        // ============================================
        // GET ROLE BY ID
        // ============================================
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetRole(int id)
        {
            var role = _roleRepository.GetRoleById(id);
            if (role == null)
                return NotFound("Role not found");

            return Ok(_mapper.Map<RoleDto>(role));
        }

        // ============================================
        // CREATE ROLE  (Audit Included)
        // ============================================
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        public IActionResult CreateRole([FromBody] RoleCreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RoleName))
                return BadRequest("RoleName cannot be empty");

            if (_roleRepository.RoleExists(dto.RoleName))
                return Conflict("Role already exists");

            var role = _mapper.Map<Role>(dto);

            if (!_roleRepository.CreateRole(role, CurrentUserId))
            {
                ModelState.AddModelError("", "Failed to create role");
                return StatusCode(500, ModelState);
            }

            return Ok("Role created successfully");
        }

        // ============================================
        // UPDATE ROLE  (Audit Included)
        // ============================================
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public IActionResult UpdateRole(int id, [FromBody] RoleCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid body");

            var role = _roleRepository.GetRoleById(id);
            if (role == null)
                return NotFound("Role not found");

            // duplicate check
            if (_roleRepository.RoleExists(dto.RoleName) &&
                role.RoleName.ToLower() != dto.RoleName.ToLower())
            {
                return Conflict("A role with this name already exists");
            }

            role.RoleName = dto.RoleName;

            if (!_roleRepository.UpdateRole(role, CurrentUserId))
            {
                ModelState.AddModelError("", "Failed to update role");
                return StatusCode(500, ModelState);
            }

            return Ok("Role updated successfully");
        }

        // ============================================
        // SOFT DELETE ROLE
        // ============================================
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult SoftDeleteRole(int id)
        {
            var role = _roleRepository.GetRoleById(id);
            if (role == null)
                return NotFound("Role not found");

            if (!_roleRepository.SoftDeleteRole(role, CurrentUserId))
            {
                ModelState.AddModelError("", "Failed to delete role");
                return StatusCode(500, ModelState);
            }

            return Ok("Role soft-deleted successfully");
        }

        // ============================================
        // RESTORE ROLE
        // ============================================
        [HttpPost("restore/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RestoreRole(int id)
        {
            var role = _roleRepository.GetRoleIncludingDeleted(id);
            if (role == null)
                return NotFound("Role not found");

            if (!role.IsDeleted)
                return BadRequest("Role is already active");

            if (!_roleRepository.RestoreRole(role))
            {
                ModelState.AddModelError("", "Failed to restore role");
                return StatusCode(500, ModelState);
            }

            return Ok("Role restored successfully");
        }
    }
}
