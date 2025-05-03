using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand,LoginUserCommandResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenAuthenticationService _tokenAuthenticationService;

        public LoginUserCommandHandler(UserManager<User> userManager, ITokenAuthenticationService tokenAuthenticationService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenAuthenticationService = tokenAuthenticationService ?? throw new ArgumentNullException(nameof(tokenAuthenticationService));
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Check if the user is verified
            if (!user.IsVerified)
            {
                throw new UnauthorizedAccessException("User is not verified. Please verify your email.");
            }

            // Check the user's password
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Generate tokens
            var roles = await _userManager.GetRolesAsync(user);
            var (accessToken, refreshToken) = _tokenAuthenticationService.GenerateTokens(user.Email, roles);

            return new LoginUserCommandResponse
            {
                Access = accessToken,
                Refresh = refreshToken,
                Role = roles
            };

        }
    }

}
