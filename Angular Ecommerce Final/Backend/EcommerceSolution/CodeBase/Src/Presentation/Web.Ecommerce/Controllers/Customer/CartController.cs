using Application.Ecom.CartOperations.Commands.AddCartItem;
using Application.Ecom.CartOperations.Commands.ClearCart;
using Application.Ecom.CartOperations.Commands.RemoveCartItem;
using Application.Ecom.CartOperations.Commands.UpdateCartItem;
using Application.Ecom.CartOperations.Queries.GetCart;
using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Ecommerce.Controllers.Customer
{
    /// <summary>
    /// Cart Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Bearer")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerManager _logger;

        public CartController(IMediator mediator, ILoggerManager logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// To Add an item to Cart
        /// </summary>
        /// <param name="command">Product Id and Quantity</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("AddtoCart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemCommand command)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarn("AddCartItem request model is invalid.");
                return BadRequest(ModelState);
            }

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                _logger.LogInfo($"Product added to cart successfully.");
                return Ok(response);
            }

            _logger.LogError($"Failed to add product to cart: {response.Message}");
            return BadRequest(response.Message);
        }

        /// <summary>
        /// To Update Quantity of an Item in Cart.
        /// </summary>
        /// <param name="command">Item Id and Quantity </param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("UpdateQuantity")]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> UpdateCartItem( [FromBody] UpdateCartItemCommand command)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarn("Invalid request.");
                return BadRequest(ModelState);
            }

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                _logger.LogInfo($"Cart item updated successfully.");
                return NoContent();
            }

            _logger.LogError($"Failed to update cart item: {response.Message}");
            return BadRequest(response.Message);
        }

        /// <summary>
        /// To Login the application.
        /// </summary>
        /// <param name="command">Login user name and password.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("RemoveitemfromCart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveCartItem(Guid id)
        {
            var response = await _mediator.Send(new RemoveCartItemCommand { CartItemId = id });
            if (response.Success)
            {
                _logger.LogInfo($"Cart item removed successfully.");
                return Ok(response);
            }

            _logger.LogError($"Failed to remove cart item: {response.Message}");
            return BadRequest(response.Message);
        }

        /// <summary>
        /// To remove all the items in Cart
        /// </summary>
        /// <param name="command">User Id</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("ClearCart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ClearCart([FromBody] ClearCartCommand command)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarn("ClearCart request model is invalid.");
                return BadRequest(ModelState);
            }

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                _logger.LogInfo($"Cart cleared successfully.");
                return NoContent();
            }

            _logger.LogError($"Failed to clear cart: {response.Message}");
            return BadRequest(response.Message);
        }

        [HttpGet("ViewCart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCart(Guid customerId)
        {
            var response = await _mediator.Send(new GetCartByCustomerIdQuery { CustomerId = customerId });
            if (response != null)
            {
                _logger.LogInfo($"Cart retrieved successfully.");
                return Ok(response);
            }

            _logger.LogWarn($"Cart not found for customer {customerId}.");
            return NotFound($"Cart not found for customer {customerId}.");
        }
    }

}
