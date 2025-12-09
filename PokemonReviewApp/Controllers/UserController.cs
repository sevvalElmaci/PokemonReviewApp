using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Security.Claims;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirst("userId").Value);

        private string CurrentUserRole =>
     User.FindFirst(ClaimTypes.Role)?.Value
     ?? "User";  // JWT’de "role" claim’in var

        // ============================================
        // GET ALL USERS  (Admin & Manager)
        // ============================================
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(_mapper.Map<List<UserDto>>(users));
        }

        // ============================================
        // GET USER BY ID
        // ============================================
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(_mapper.Map<UserDto>(user));
        }

        // ============================================
        // GET DELETED USERS (Admin only)
        // ============================================
        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDeletedUsers()
        {
            var deleted = _userRepository.GetDeletedUsers();
            return Ok(_mapper.Map<List<UserDto>>(deleted));
        }

        // ============================================
        // CREATE USER  (Hierarchy controlled)
        // ============================================
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(403)]
        public IActionResult CreateUser([FromBody] UserCreateDto request)
        {
            if (request == null)
                return BadRequest("Body is empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = _roleRepository.GetRoleById(request.RoleId);
            if (role == null)
                return BadRequest("Role not found");

            // ----------------------------
            // HIERARCHY RULES
            // ----------------------------
            if (CurrentUserRole == "User")
                return Forbid("Users cannot create anyone.");

            if (CurrentUserRole == "Manager" && role.RoleName == "Admin")
                return Forbid("Manager cannot create Admin.");

            // Username duplicate check
            var exists = _userRepository
                .GetUsers()
                .Any(u => u.UserName.ToLower() == request.Username.ToLower());

            if (exists)
                return Conflict("Username already exists");

            // Create
            var user = _mapper.Map<User>(request);

            bool created = _userRepository.CreateUser(user, CurrentUserId);
            if (!created)
            {
                ModelState.AddModelError("", "Failed to create user.");
                return StatusCode(500, ModelState);
            }

            return Ok("User created successfully.");
        }

        // ============================================
        // UPDATE USER (Admin + Manager)
        // ============================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Empty body");

            if (dto.Id != id)
                return BadRequest("ID mismatch");

            var user = _userRepository.GetUserIncludingDeleted(id);
            if (user == null)
                return NotFound("User not found");

            // Manager cannot update admin
            if (CurrentUserRole == "Manager" && user.Role?.RoleName == "Admin")
                return Forbid("Manager cannot update Admin.");
            // Manager cannot assign Admin role to anyone (including themselves)
            if (CurrentUserRole == "Manager" && dto.RoleId == 1)
                return Forbid("Manager cannot assign Admin role.");


            // Username duplicate (except self)
            var normalized = dto.Username?.ToLower();
            var exists = _userRepository
                .GetUsers()
                .FirstOrDefault(u => u.UserName.ToLower() == normalized);

            if (exists != null && exists.Id != id)
                return Conflict("Username already exists");

            // Update allowed fields
            user.UserName = dto.Username ?? user.UserName;
            user.Password = string.IsNullOrWhiteSpace(dto.Password)
                            ? user.Password
                            : dto.Password;

            user.RoleId = dto.RoleId;
            bool updated = _userRepository.UpdateUser(user, CurrentUserId);

            if (!updated)
            {
                ModelState.AddModelError("", "Failed to update user");
                return StatusCode(500, ModelState);
            }

            return Ok("User updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult SoftDeleteUser(int id)
        {
            var targetUser = _userRepository.GetUserById(id);
            if (targetUser == null)
                return NotFound("User not found");

            // Manager cannot delete Admin
            if (CurrentUserRole == "Manager" && targetUser.RoleId == 1)
                return Forbid("Manager cannot delete an Admin.");

            // Manager cannot delete another Manager
            if (CurrentUserRole == "Manager" && targetUser.RoleId == 2)
                return Forbid("Manager cannot delete another Manager.");

            // OK → delete
            bool deleted = _userRepository.SoftDeleteUser(targetUser, CurrentUserId);

            if (!deleted)
            {
                ModelState.AddModelError("", "Failed to delete user.");
                return StatusCode(500, ModelState);
            }

            return Ok("User soft-deleted successfully.");
        }


        // ============================================
        // RESTORE USER (NO AUDIT AS YOU WANTED)
        // ============================================
        [HttpPost("restore/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RestoreUser(int id)
        {
            var user = _userRepository.GetUserIncludingDeleted(id);
            if (user == null)
                return NotFound("User not found.");

            if (!user.IsDeleted)
                return BadRequest("User is already active.");

            bool restored = _userRepository.RestoreUser(user);

            if (!restored)
            {
                ModelState.AddModelError("", "Failed to restore user.");
                return StatusCode(500, ModelState);
            }

            return Ok("User restored successfully.");
        }

        [HttpPost("create-with-log")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserWithLog([FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Body is empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = _roleRepository.GetRoleById(dto.RoleId);
            if (role == null)
                return BadRequest("Role not found");

            // Hierarchy check
            if (CurrentUserRole == "User")
                return Forbid("Users cannot create anyone.");

            if (CurrentUserRole == "Manager" && role.RoleName == "Admin")
                return Forbid("Manager cannot create Admin.");

            // Duplicate username check
            var existing = _userRepository.GetUserByUserName(dto.Username);
            if (existing != null)
                return Conflict("This username already exists.");

            // Map
            var userEntity = _mapper.Map<User>(dto);

            // Logged creator
            userEntity.CreatedUserId = CurrentUserId;
            userEntity.CreatedDateTime = DateTime.Now;
            userEntity.UpdatedUserId = CurrentUserId;
            userEntity.UpdatedDateTime = DateTime.Now;

            // === CREATE WITH LOG FUNCTION ===
            var createdUser = _userRepository.CreateUserWithLog(userEntity);

            if (createdUser == null)
            {
                ModelState.AddModelError("", "Failed to create user with log.");
                return StatusCode(500, ModelState);
            }

            return Ok("User created successfully WITH log.");
        }
    }
}
