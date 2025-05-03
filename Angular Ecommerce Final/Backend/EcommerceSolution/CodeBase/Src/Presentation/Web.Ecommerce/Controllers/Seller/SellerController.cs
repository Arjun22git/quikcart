using Application.Ecom.Interfaces;
using Application.Ecom.OrderOperations.Commands.UpdateOrderStatus;
using Application.Ecom.OrderOperations.Queries.GetAllOrders;
using Application.Ecom.OrderOperations.Queries.GetSellerOrders;
using Application.Ecom.OrderOperations.Queries.TrackOrder;
using Application.Ecom.ProductOperations.Queries.GetProductsBySeller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Ecommerce.Controllers.Seller
{
    /// <summary>
    /// Seller Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SellerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerManager _logger;

        public SellerController(IMediator mediator, ILoggerManager logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get All Orders of a particular Seller which includes an item sold by that Seller
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        [HttpGet("GetOrdersofSeller")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetSellerOrdersQuery(Guid SellerId)
        {
            var response = await _mediator.Send(new GetSellerOrdersQuery { SellerId = SellerId});
            if (response != null)
            {
                return Ok(response);
            }

            _logger.LogWarn("No Orders Available") ;
            return NotFound();
        }

        /// <summary>
        /// Get All Products of a particular Seller
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        [HttpGet("GetProductsofSeller")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetSellerProductsQuery(Guid SellerId)
        {
            var response = await _mediator.Send(new GetSellerProductsQuery { SellerId = SellerId });
            if (response != null)
            {
                return Ok(response);
            }

            _logger.LogWarn("No Products Available");
            return NotFound();
        }


        /// <summary>
        /// Update the status of an Order - Shipped,Delivered etc
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateOrderStatus")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusCommand command)
        {
            if (orderId != command.OrderId)
            {
                _logger.LogWarn("Order ID mismatch.");
                return BadRequest("Order ID mismatch.");
            }

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                _logger.LogInfo($"Order {orderId} status updated successfully.");
                return NoContent();
            }

            _logger.LogError(response.Message);
            return BadRequest(response.Message);
        }

    }
}
