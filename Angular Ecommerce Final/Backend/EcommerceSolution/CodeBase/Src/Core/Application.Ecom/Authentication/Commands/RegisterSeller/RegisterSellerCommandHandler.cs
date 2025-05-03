using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Ecom.Authentication.Commands.RegisterSeller
{
    public class RegisterSellerCommandHandler : IRequestHandler<RegisterSellerCommand, RegisterSellerResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IOtpService _otpService;
        private readonly IEcommerceDbContext _dbContext; // Inject DbContext
        private readonly ILoggerManager _logger; // Added logger for better error handling and info

        public RegisterSellerCommandHandler(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IOtpService otpService,
            IEcommerceDbContext dbContext, // Inject DbContext
            ILoggerManager logger) // Inject logger
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RegisterSellerResponse> Handle(RegisterSellerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Processing registration for {request.Email}");

            // Check if the passwords match
            if (request.Password != request.ConfirmPassword)
            {
                _logger.LogWarn("Passwords do not match.");
                return new RegisterSellerResponse
                {
                    Success = false,
                    Message = "Passwords do not match"
                };
            }

            // Ensure the "Seller" role exists
            var roleName = "Seller";
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                _logger.LogWarn($"Role '{roleName}' does not exist.");
                return new RegisterSellerResponse
                {
                    Success = false,
                    Message = $"Role '{roleName}' does not exist."
                };
            }

            // Create a new user
            var user = new User { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Assign the user to the "Seller" role
                var roleAssignmentResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!roleAssignmentResult.Succeeded)
                {
                    _logger.LogError($"Failed to assign role '{roleName}' to user {request.Email}: {string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description))}");
                    return new RegisterSellerResponse
                    {
                        Success = false,
                        Message = "Failed to assign role."
                    };
                }

                // Generate and send OTP
                var otp = await _otpService.GenerateOtpAsync(request.Email);
                await _otpService.SendOtpEmailAsync(request.Email, otp);

                // Create a new Seller entry
                var seller = new Seller
                {
                    SellerId = Guid.NewGuid(),
                    UserId = user.Id,
                    StoreName = request.StoreName,
                    StoreDescription = request.StoreDescription,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Use the DbContext to add the seller and save changes
                _dbContext.Sellers.Add(seller);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInfo($"Registration successful for {request.Email}. OTP sent and seller created.");

                return new RegisterSellerResponse
                {
                    Success = true,
                    Message = "Seller registered successfully. Check your email for the OTP.",
                    Email = request.Email
                };
            }

            // Log errors if user creation fails
            _logger.LogError($"Failed to create user {request.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            return new RegisterSellerResponse
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
    }




}