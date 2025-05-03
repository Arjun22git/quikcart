using Application.Ecom.Authentication.Commands.GenerateAccess;
using Application.Ecom.Authentication.Commands.LoginUser;
using Application.Ecom.Authentication.Commands.RegisterCustomer;
using Application.Ecom.Authentication.Commands.RegisterSeller;
using Application.Ecom.Authentication.Commands.VerifyOTP;
using Application.Ecom.Authentication.Queries.ViewAllUsers;
using Application.Ecom.Authentication.Queries.ViewCustomerbyEmail;
using Application.Ecom.Authentication.Queries.ViewSellerbyEmail;
using Application.Ecom.CartOperations.Queries.GetCart;
using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace Web.Ecommerce.Controllers.Account
{
    /// <summary>
    /// Account Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerManager _logger;

        public AccountController(IMediator mediator, ILoggerManager logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// To Register as a Customer.
        /// </summary>
        /// <param name="command">Username/Email,Password and Password Confirmation.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand command)
        {
            if (command == null)
            {
                _logger.LogWarn("RegisterCustomer command is null.");
                return BadRequest("Invalid registration request");
            }

            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return Ok(new RegisterCustomerResponse
                {
                    Success = true,
                    Message = "Customer registered successfully. Check your email for the OTP.",
                    Email = response.Email
                });
            }

            _logger.LogWarn(response.Message);
            return BadRequest(new RegisterCustomerResponse
            {
                Success = false,
                Message = response.Message
            });
        }

        /// <summary>
        /// To Register as a Seller.
        /// </summary>
        /// <param name="command">Register Email,Password,Store name and Description.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("RegisterSeller")]
        public async Task<IActionResult> RegisterSeller([FromBody] RegisterSellerCommand command)
        {
            if (command == null)
            {
                _logger.LogWarn("RegisterSeller command is null.");
                return BadRequest("Invalid registration request");
            }

            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return Ok(new RegisterSellerResponse
                {
                    Success = true,
                    Message = "Seller registered successfully. Check your email for the OTP.",
                    Email = response.Email
                });
            }

            _logger.LogWarn(response.Message);
            return BadRequest(new RegisterSellerResponse
            {
                Success = false,
                Message = response.Message
            });
        }

        /// <summary>
        /// To Verify the Generated OTP.
        /// </summary>
        /// <param name="command">OTP generated and Sent via mail</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command)
        {
            if (command == null)
            {
                _logger.LogWarn("VerifyOtp command is null.");
                return BadRequest("Invalid OTP verification request");
            }

            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return Ok(new
                {
                    Message = "OTP verified successfully. You can now log in.",
                    Success = true
                });
            }

            _logger.LogWarn(response.Message);
            return BadRequest(new
            {
                Message = response.Message,
                Success = false
            });
        }


        /// <summary>
        /// To Login the application.
        /// </summary>
        /// <param name="command">Login user name and password.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Access != null && response.Refresh != null)
            {

                return Ok(new
                {
                    AccessToken = response.Access,
                    RefreshToken = response.Refresh,
                    Roles =response.Role

                });
            }

            _logger.LogWarn("Invalid credentials");
            return Unauthorized("Invalid credentials");
        }


        /// <summary>
        /// To view customer details.
        /// </summary>
        /// <param name="command">Login user name and password.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("Customer")]
        public async Task<IActionResult> ViewCustomer(string email)
        {
            var response = await _mediator.Send(new ViewCustomerQuery { Email = email});
            if (response != null)
            {
                _logger.LogInfo($"Customer retrieved successfully.");
                return Ok(response);
            }

            _logger.LogWarn("Invalid credentials");
            return Unauthorized("Invalid credentials");
        }



        /// <summary>
        /// To view all details.
        /// </summary>
        /// <param name="command">Login user name and password.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("ViewAllUsers")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ViewAllUsers()
        {
            var response = await _mediator.Send(new ViewAllUsersQuery());
            if (response != null)
            {
                _logger.LogInfo($"Customer retrieved successfully.");
                return Ok(response);
            }

            _logger.LogWarn("Invalid credentials");
            return Unauthorized("Invalid credentials");
        }


        /// <summary>
        /// To get sellerId from email.
        /// </summary>
        /// <param name="command">Login user name and password.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("GetSellerId")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> ViewSellerId(string Seller)
        {
            var response = await _mediator.Send(new ViewSellerbyEmailQuery { Email = Seller });
            if (response != null)
            {
                _logger.LogInfo($"Seller ID retrieved successfully.");
                return Ok(response);
            }

            _logger.LogWarn("Invalid credentials");
            return Unauthorized("Invalid credentials");
        }




        /// <summary>
        /// To Generate New Access Token 
        /// </summary>
        /// <param name="command">Previous Refresh Token.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("GenerateNewAccessToken")]
        public async Task<IActionResult> GenerateAccess([FromBody] GenerateAccessTokenCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.RefreshToken))
            {
                _logger.LogWarn("Refresh token is missing or invalid in the request.");
                return BadRequest(new
                {
                    message = "Refresh token is required."
                });
            }

            try
            {
                var newAccessToken = await _mediator.Send(command);

                if (string.IsNullOrWhiteSpace(newAccessToken))
                {
                    _logger.LogWarn("Invalid or expired refresh token.");
                    return Unauthorized(new
                    {
                        message = "Invalid or expired refresh token."
                    });
                }

                // Return a successful response with the new access token
                return Ok(new
                {
                    accessToken = newAccessToken,
                    expiration = DateTime.UtcNow.AddMinutes(30).ToString("o"), // Assuming 30-minute expiry
                    message = "New access token generated successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while generating access token: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "An internal server error occurred."
                });
            }
        }
    }
}
