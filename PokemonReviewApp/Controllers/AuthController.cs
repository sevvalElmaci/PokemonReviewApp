using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public AuthController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto login)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == login.UserName);

            if (user == null)
                return Unauthorized("User not found");

            if (user.Password != login.Password)
                return Unauthorized("Invalid password");


            var roleName = user.Role.RoleName;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
new Claim(type: ClaimTypes.Name, value: user.UserName),
new Claim(type: ClaimTypes.Role, value: roleName)
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { token = tokenString });
        }





    }



}
