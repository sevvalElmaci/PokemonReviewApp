using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Authorization;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto login)
        {
            if (login == null)
                return BadRequest("Body is empty.");

            // USER + ROLE
            var user = _userRepository.GetUserWithRole(login.UserName);
            if (user == null)
                return Unauthorized("User not found.");

            // PASSWORD CHECK (SHA-512 + SALT)
            var computedHash = Sha512Hasher.HashPassword(login.Password, user.PasswordSalt);
            if (computedHash != user.PasswordHash)
                return Unauthorized("Invalid username or password.");

            // ---- PERMISSIONS ----
            var userPermissions = _userRepository.GetUserPermissions(user.Id);

            // ---- CLAIMS (RawToken ile birebir uyumlu) ----
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),         // RawToken okuyor
                new Claim("role", user.Role.RoleName),         
                new Claim("unique_name", user.UserName)       
            };

            // Permission claimleri
            foreach (var perm in userPermissions)
                claims.Add(new Claim("permission", perm.PermissionName));

            // ---- SIGNING KEY ----
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                // 💛 1 YILLIK TOKEN
                Expires = DateTime.UtcNow.AddYears(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            var tokenString = handler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                username = user.UserName,
                role = user.Role.RoleName,
                permissions = userPermissions.Select(p => p.PermissionName).ToList()
            });
        }
    }
}
