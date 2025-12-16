using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Authorization;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Security.Claims;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
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

        private int CurrentUserId => int.Parse(User.FindFirst("userId").Value);
        private string CurrentUserRole => User.FindFirst(ClaimTypes.Role)?.Value ?? "User";


        // ============================================
        // GET ALL USERS (NOT DELETED)
        // ============================================
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();

            var result = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.UserName,
                RoleId = u.RoleId,
                RoleName = u.Role.RoleName
            }).ToList();

            return Ok(result);
        }



        // ============================================
        // GET ALL DELETED USERS
        // ============================================
        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDeletedUsers()
        {
            var users = _userRepository.GetDeletedUsers();
            return Ok(users);
        }


        // ============================================
        // GET USER BY ID
        // ============================================
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
                return NotFound("User not found.");

            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName
            };

            return Ok(dto);
        }


        // ============================================
        // CREATE USER WITH LOG (Transactional)
        // ============================================
        [HttpPost("create-with-log")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult CreateUserWithLog([FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Body is empty.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = _roleRepository.GetRoleById(dto.RoleId);
            if (role == null)
                return BadRequest("Role not found.");

            // ROLE HIERARCHY CONTROL
            if (CurrentUserRole == "Manager" && role.RoleName == "Admin")
                return Forbid("Manager cannot create Admin.");

            // Duplicate username
            var exists = _userRepository.GetUserByUserName(dto.Username);
            if (exists != null)
                return Conflict("Username already exists.");

            // Password hashing
            var salt = Sha512Hasher.GenerateSalt();
            var hash = Sha512Hasher.HashPassword(dto.Password, salt);

            var user = new User
            {
                UserName = dto.Username,
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleId = dto.RoleId,
                CreatedUserId = CurrentUserId,
                CreatedDateTime = DateTime.Now,
                UpdatedUserId = CurrentUserId,
                UpdatedDateTime = DateTime.Now,
                IsDeleted = false
            };

            try
            {
                var createdUser = _userRepository.CreateUserWithLog(user);
                return Ok("User created successfully WITH log.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while creating user: {ex.Message}");
            }
        }


        // ============================================
        // UPDATE USER (WITH LOG)
        // ============================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
                return NotFound("User not found.");

            var newRole = _roleRepository.GetRoleById(dto.RoleId);
            if (newRole == null)
                return BadRequest("Role not found.");

            // MANAGER CANNOT UPDATE AN ADMIN
            if (CurrentUserRole == "Manager" && user.Role.RoleName == "Admin")
                return Forbid("Manager cannot update an Admin.");

            // Manager cannot assign Admin role
            if (CurrentUserRole == "Manager" && newRole.RoleName == "Admin")
                return Forbid("Manager cannot assign Admin role.");

            // ============================
            // PASSWORD CHANGE (OPTIONAL)
            // ============================
            var wantsPasswordChange = !string.IsNullOrWhiteSpace(dto.NewPassword);

            if (wantsPasswordChange)
            {
                // newPassword girildiyse oldPassword zorunlu
                if (string.IsNullOrWhiteSpace(dto.OldPassword))
                    return BadRequest("OldPassword is required to change password.");

                // old password doğrula: mevcut salt ile hashle ve karşılaştır
                var oldHash = Sha512Hasher.HashPassword(dto.OldPassword, user.PasswordSalt);

                if (oldHash != user.PasswordHash)
                    return Forbid("Old password is incorrect. Password change denied.");

                // doğruysa yeni salt+hash üret
                var newSalt = Sha512Hasher.GenerateSalt();
                var newHash = Sha512Hasher.HashPassword(dto.NewPassword, newSalt);

                user.PasswordSalt = newSalt;
                user.PasswordHash = newHash;
            }

            // ============================
            // OTHER UPDATES
            // ============================
            user.UserName = dto.Username;
            user.RoleId = dto.RoleId;

            var success = _userRepository.UpdateUser(user, CurrentUserId);
            if (!success)
                return StatusCode(500, "Failed to update user.");

            return Ok(wantsPasswordChange
                ? "User updated successfully (password changed)."
                : "User updated successfully.");
        }



        // ============================================
        // SOFT DELETE USER (WITH LOG)
        // ============================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult SoftDeleteUser(int id)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
                return NotFound("User not found.");

            if (CurrentUserRole == "Manager" && user.Role.RoleName == "Admin")
                return Forbid("Manager cannot delete an Admin.");

            var result = _userRepository.SoftDeleteUser(user, CurrentUserId);

            if (!result)
                return StatusCode(500, "Failed to delete user.");

            return Ok("User soft-deleted successfully.");
        }


        // ============================================
        // RESTORE USER (WITH LOG)
        // ============================================
        [HttpPost("restore/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RestoreUser(int id)
        {
            var user = _userRepository.GetUserIncludingDeleted(id);
            if (user == null)
                return NotFound("User not found.");

            var result = _userRepository.RestoreUser(user);

            if (!result)
                return StatusCode(500, "Failed to restore user.");

            return Ok("User restored successfully.");
        }
    }
}
