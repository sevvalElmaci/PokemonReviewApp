using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace PokemonReviewApp.Authorization
{
    public class RawTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public RawTokenAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            var rawToken = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(rawToken))
                return Task.FromResult(AuthenticateResult.Fail("Empty Authorization Header"));

            if (rawToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                rawToken = rawToken.Substring("Bearer ".Length).Trim();
            }

            try
            {
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("SuperDuperCuperMEGASecretKey3!14159265358979");

                tokenHandler.ValidateToken(rawToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                var jwt = (JwtSecurityToken)validatedToken;

                 
                var identity = new ClaimsIdentity(jwt.Claims, Scheme.Name);

                //Manual adding for specific claims
                identity.AddClaim(new Claim(ClaimTypes.Role,
                    jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? ""));

                identity.AddClaim(new Claim("userId",
                    jwt.Claims.FirstOrDefault(c => c.Type == "userId")?.Value ?? ""));

                identity.AddClaim(new Claim("unique_name",
                    jwt.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value ?? ""));

                // Permissions
                var permissions = jwt.Claims.Where(c => c.Type == "permission");
                foreach (var perm in permissions)
                {
                    identity.AddClaim(new Claim("permission", perm.Value));
                }

                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail($"Token validation failed: {ex.Message}"));
            }
        }
    }
}
