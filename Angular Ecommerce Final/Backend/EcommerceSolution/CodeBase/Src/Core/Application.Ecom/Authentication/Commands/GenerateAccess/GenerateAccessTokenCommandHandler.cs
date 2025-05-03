using Application.Ecom.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.GenerateAccess
{


    public class GenerateAccessTokenCommandHandler : IRequestHandler<GenerateAccessTokenCommand, string>
    {
        private readonly ITokenAuthenticationService _tokenService;
        private readonly ILoggerManager _logger;

        public GenerateAccessTokenCommandHandler(ITokenAuthenticationService tokenService, ILoggerManager logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            // Generate a new access token from the provided refresh token
            var newAccessToken = await _tokenService.GenerateAccessTokenFromRefreshToken(request.RefreshToken);

            // Log the action
            if (newAccessToken != null)
            {
                _logger.LogInfo("New access token generated successfully.");
            }
            else
            {
                _logger.LogWarn("Failed to generate new access token from refresh token.");
            }

            // Return response
            return newAccessToken;
        }
    }
}