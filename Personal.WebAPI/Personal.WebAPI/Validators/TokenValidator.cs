using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Personal.WebAPI.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Personal.WebAPI.Validators
{
    public class TokenValidator
    {
        private readonly RequestDelegate _next;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string[] _exemptEndpoints;

        public TokenValidator(RequestDelegate next, IOptions<JwtConfig> jwtOptions, string[] exemptEndpoints)
        {
            _next = next;
            _key = jwtOptions.Value.Key;
            _issuer = jwtOptions.Value.Issuer;
            _audience = jwtOptions.Value.Audience;
            _exemptEndpoints = exemptEndpoints.Select(e => e.ToLower()).ToArray(); // Normalize for case-insensitive matching
        }

        public async Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path.Value?.ToLower();

            // Check if the current path is exempt
            if (_exemptEndpoints.Any(endpoint => requestPath != null && requestPath.StartsWith(endpoint)))
            {
                await _next(context);
                return;
            }

            // Check for Authorization header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization token is missing.");
                return;
            }

            // Validate the token
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out _);

                // Token is valid, proceed to the next middleware
                await _next(context);
            }
            catch
            {
                // Token validation failed
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token.");
            }
        }
    }
}
