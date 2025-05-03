using Application.Ecom.CartOperations.Commands.CreateEmptyCart;
using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.RegisterCustomer

{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, RegisterCustomerResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IOtpService _otpService;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ILoggerManager _logger;
        private readonly IMediator _mediator;

        public RegisterCustomerCommandHandler(UserManager<User> userManager, IOtpService otpService, RoleManager<IdentityRole<Guid>> roleManager, ILoggerManager logger, IMediator mediator)
        {
            _userManager = userManager;
            _otpService = otpService;
            _roleManager = roleManager;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<RegisterCustomerResponse> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
            {
                _logger.LogWarn("Password and confirm password do not match.");
                return new RegisterCustomerResponse
                {
                    Success = false,
                    Message = "Passwords do not match"
                };
            }

            // Check if the "Customer" role exists; if not, create it
            var roleName = "Customer";
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (!roleCreationResult.Succeeded)
                {
                    _logger.LogError("Failed to create role: " + string.Join(", ", roleCreationResult.Errors.Select(e => e.Description)));
                    return new RegisterCustomerResponse
                    {
                        Success = false,
                        Message = "Failed to create role"
                    };
                }
                _logger.LogInfo("Role created: " + roleName);
            }

            var user = new User { UserName = request.Name, Email = request.Email, CreatedAt=DateTime.UtcNow ,UpdatedAt=DateTime.UtcNow };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Assign the user to the "Customer" role
                var roleAssignmentResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!roleAssignmentResult.Succeeded)
                {
                    _logger.LogError("Failed to assign role: " + string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description)));
                    return new RegisterCustomerResponse
                    {
                        Success = false,
                        Message = "Failed to assign role"
                    };
                }

                _logger.LogInfo("User created and assigned role: " + roleName);

                // Create an empty cart for the customer
                var cartResponse = await _mediator.Send(new CreateEmptyCartCommand { CustomerId = user.Id });
                if (!cartResponse.Success)
                {
                    _logger.LogError("Failed to create an empty cart.");
                    return new RegisterCustomerResponse
                    {
                        Success = false,
                        Message = "Customer registered, but failed to create an empty cart."
                    };
                }

                // Generate and send OTP
                var otp = await _otpService.GenerateOtpAsync(request.Email);
                await _otpService.SendOtpEmailAsync(request.Email, otp);

                return new RegisterCustomerResponse
                {
                    Success = true,
                    Message = "Customer registered successfully. Check your email for the OTP.",
                    Email = request.Email
                };
            }

            _logger.LogError("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            return new RegisterCustomerResponse
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
    }




}
