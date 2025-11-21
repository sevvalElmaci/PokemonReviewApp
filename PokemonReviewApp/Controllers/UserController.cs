using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Helpers;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Net.Mime;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository, IMapper mapper, IRoleRepository role, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _roleRepository= role;

        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var users = _userRepository.GetUsers();
            var result = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<UserDto> CreateUser([FromBody] UserCreateDto request)
        {
            if (request is null)
                return BadRequest("Cant be null");
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var role = _roleRepository.GetRoleById(request.RoleId);
            if (role == null)
                return BadRequest("Role not found");
            var allUsers = _userRepository.GetUsers();

            // Duplicate username check
            bool usernameExists = DuplicateCheckHelper.ExistsDuplicate(
                allUsers,
                u => u.UserName,
                request.Username,
                0,                 // create olduğu için currentId = 0
                u => u.Id
            );

            if (usernameExists)
            {
                return Conflict("this user name already exists. please choose another user name");
            }
                
            try
            {
                var userEntity = _mapper.Map<User>(request);
                var createdUser = _userRepository.CreateUserWithLog(userEntity);
                var response = _mapper.Map<UserDto>(createdUser);
                var location = $"{Request.Scheme}://{Request.Host}/api/users";
                return Created(location, response);

            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation failed while creating user {@Request}", request);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflict while creating user {@Request}", request);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating user {@Request}", request);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString()); // 👈 TAM HATA BURADA GÖZÜKECEK
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult SoftDeleteUser (int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound("User not found");

            _userRepository.SoftDeleteUser(user);

            return Ok("User soft-deleted");
        }

        [Authorize]
        [HttpPost("restore/{id}")]
        public IActionResult RestoreUser(int id)
        {
            var user = _userRepository.GetUserIncludingDeleted(id);
            if (user == null)
                return NotFound("User not found");
            if (!user.IsDeleted)
                return BadRequest("User is not deleted");

            _userRepository.RestoreUser(user);
            return Ok("User restored");
        }
        [Authorize]
        [HttpGet("deleted")]
        public IActionResult GetDeletedUsers()
        {
            var deleted = _userRepository.GetDeletedUsers();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(deleted));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(404)]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(_mapper.Map<UserDto>(user));
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Body cannot be empty");

            if (dto.Id != id)
                return BadRequest("ID mismatch");

            var user = _userRepository.GetUserIncludingDeleted(id);
            if (user == null)
                return NotFound("User not found");

            // Duplicate username check
            var existingUser = _userRepository.GetUserByUserName(dto.Username);
            if (existingUser != null && existingUser.Id != id)
                return Conflict("Another user with this username already exists");

            // Role check
            var role = _roleRepository.GetRoleById(dto.RoleId);
            if (role == null)
                return BadRequest("Role not found");

            // ---- UPDATE FIELDS ----
            user.UserName = dto.Username ?? user.UserName;
            user.Password = dto.Password ?? user.Password;
            user.RoleId = dto.RoleId;

            user.UpdatedDateTime = DateTime.Now;
            user.UpdatedUserId = 1; // JWT gelince değişecek

            _userRepository.UpdateUser(user);

            return Ok("User updated successfully");
        }



    }
}
