using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Net.Mime;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository, IMapper mapper, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
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
    }
}
