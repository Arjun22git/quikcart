using Application.Ecom.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    public class TokenAuthenticationService : ITokenAuthenticationService
    {

        private readonly IConfiguration _configuration;
        private readonly string _accessTokenSecretKey;
        private readonly string _refreshTokenSecretKey;
        private readonly ILoggerManager _logger;

        public TokenAuthenticationService(IConfiguration configuration, ILoggerManager logger)
        {
            _configuration = configuration;
            _accessTokenSecretKey = _configuration["Jwt:Key"];
            _refreshTokenSecretKey = _configuration["Jwt:RefreshTokenKey"];
            _logger = logger;
        }

        public (string AccessToken, string RefreshToken) GenerateTokens(string username, IList<string> roles)
        {
            var accessToken = GenerateToken(username, roles, _accessTokenSecretKey, TimeSpan.FromMinutes(30));
            var refreshToken = GenerateToken(username, roles, _refreshTokenSecretKey, TimeSpan.FromDays(7));
            return (AccessToken: accessToken, RefreshToken: refreshToken);
        }

        public string GenerateToken(string username, IList<string> roles, string secretKey, TimeSpan expiry)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(expiry),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetAccessTokenPrincipal(string token)
        {
            return GetPrincipal(token, _accessTokenSecretKey);
        }

        public ClaimsPrincipal GetRefreshTokenPrincipal(string token)
        {
            return GetPrincipal(token, _refreshTokenSecretKey);
        }

        private ClaimsPrincipal GetPrincipal(string token, string secretKey)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey);

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogError("Security Token Expired");
                return null;
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                _logger.LogError("Invalid Signature");
                return null;
            }
            catch (SecurityTokenInvalidLifetimeException)
            {
                _logger.LogError("Token is missing an expiration time or is not yet valid");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred: {ex.Message}");
                return null;
            }
        }

        public string ValidateToken(string token)
        {
            var principal = GetAccessTokenPrincipal(token);
            if (principal == null)
                return null;

            var identity = principal.Identity as ClaimsIdentity;
            var usernameClaim = identity?.FindFirst(ClaimTypes.Name);
            return usernameClaim?.Value;
        }

        public string ValidateRefreshToken(string token)
        {
            var principal = GetRefreshTokenPrincipal(token);
            if (principal == null)
                return null;

            var identity = principal.Identity as ClaimsIdentity;
            var usernameClaim = identity?.FindFirst(ClaimTypes.Name);
            return usernameClaim?.Value;
        }

        public async Task<string> GenerateAccessTokenFromRefreshToken(string refreshToken)
        {
            var principal = GetRefreshTokenPrincipal(refreshToken);
            if (principal == null)
            {
                _logger.LogWarn("Invalid or expired refresh token provided.");
                return null;
            }

            var username = ValidateRefreshToken(refreshToken);
            if (username == null)
            {
                _logger.LogWarn("Failed to validate refresh token.");
                return null;
            }

            var roles = principal.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList();
            var newAccessToken = GenerateToken(username, roles, _accessTokenSecretKey, TimeSpan.FromMinutes(30));
            _logger.LogInfo("New access token generated successfully.");
            return newAccessToken;
        }
    }
}

