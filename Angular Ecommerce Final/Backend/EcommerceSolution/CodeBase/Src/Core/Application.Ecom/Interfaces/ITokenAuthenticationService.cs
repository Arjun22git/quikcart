using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Interfaces
{
    public interface ITokenAuthenticationService
    {
        (string AccessToken, string RefreshToken) GenerateTokens(string username, IList<string> roles);


        string GenerateToken(string username, IList<string> roles, string secretKey, TimeSpan expiresIn);



        ClaimsPrincipal GetAccessTokenPrincipal(string token);


        ClaimsPrincipal GetRefreshTokenPrincipal(string token);


        string ValidateToken(string token);


        string ValidateRefreshToken(string token);

        Task<string> GenerateAccessTokenFromRefreshToken(string refreshToken);
    }
}
