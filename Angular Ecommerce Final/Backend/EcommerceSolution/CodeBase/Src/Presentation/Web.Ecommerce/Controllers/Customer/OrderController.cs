using Application.Ecom.Interfaces;
using Application.Ecom.OrderOperations.Commands.CancelOrder;
using Application.Ecom.OrderOperations.Commands.PlaceOrder;
using Application.Ecom.OrderOperations.Commands.ProcessPayment;
using Application.Ecom.OrderOperations.Commands.ReturnOrder;
using Application.Ecom.OrderOperations.Queries.GetAllOrders;
using Application.Ecom.OrderOperations.Queries.GetOrderById;
using Application.Ecom.OrderOperations.Queries.TrackOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Ecommerce.Controllers.Customer
{
    /// <summary>
    /// Order Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [AllowAnonymous]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerManager _logger;

        public OrderController(IMediator mediator, ILoggerManager logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Place a new Order - Customer
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("PlaceOrder")]
        [Authorize(Roles = "Customer")]
        [AllowAnonymous]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            if (command == null)
            {
                _logger.LogWarn("PlaceOrder command is null.");
                return BadRequest("Invalid order request.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                _logger.LogInfo($"Order placed successfully with ID {response.OrderId}.");
                return Ok(response);
            }

            _logger.LogError($"Failed to place order: {response.Message}");
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Track the order -Customer
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("View/TrackOrderDetails")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> TrackOrderQuery(Guid orderId)
        {
            var response = await _mediator.Send(new TrackOrderQuery { OrderId = orderId });
            if (response.Success)
            {
                return Ok(response);
            }

            _logger.LogWarn(response.Message);
            return NotFound(response.Message);
        }

        /// <summary>
        /// View All the Orders - Admin Only
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrdersQuery()
        {
            var response = await _mediator.Send(new GetAllOrdersQuery());
            if (response != null)
            {
                return Ok(response);
            }

            _logger.LogWarn("No Orders Available");
            return NotFound();
        }


        /// <summary>
        /// Search an Order By Id
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpGet("GetOrderbyId")]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderById(Guid OrderId)
        {
            var response = await _mediator.Send(new GetOrderByIdQuery { OrderId = OrderId});
            if (response != null)
            {
                return Ok(response);
            }

            _logger.LogWarn("Order not Found");
            return NotFound();
        }


        /// <summary>
        /// Process the Payment of an order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("Payment")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);

                if (response != null)
                {
                    _logger.LogInfo($"Payment for order {command.OrderId} processed successfully.");
                    return Ok(response); 
                }
                _logger.LogError($"Failed to process payment for order {command.OrderId}");
                return BadRequest("Failed to process payment.");
            }
            catch (Exception ex)
            {
                
                _logger.LogError($"An error occurred while processing payment for order {command.OrderId}.");
                return StatusCode(500, "An error occurred while processing the payment.");
            }
        }

        /// <summary>
        /// Process the return of an order
        /// </summary>
        /// <param name="orderId">The ID of the order to return</param>
        /// <param name="command">The command containing return details</param>
        /// <returns>A response indicating the result of the operation</returns>
        [HttpPost("ReturnOrder")]
        [Authorize(Roles = "Customer")]
        [AllowAnonymous]
        public async Task<IActionResult> ReturnOrder(Guid orderId, [FromBody] ReturnOrderCommand command)
        {
            if (orderId != command.OrderId)
            {
                _logger.LogWarn($"Order ID mismatch. URL Order ID: {orderId}, Command Order ID: {command.OrderId}");
                return BadRequest("Order ID mismatch.");
            }

            var response = await _mediator.Send(command);

            if (response.Success)
            {
                _logger.LogInfo($"Order return successfully initiated for Order ID: {orderId}");
                return Ok(response);
            }

            _logger.LogError($"Failed to initiate order return: {response.Message}");
            return BadRequest(response.Message);
        }


        /// <summary>
        /// Cancel a placed Order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("CancelOrder")]
        //[Authorize(Roles = "Customer")]
        [AllowAnonymous]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var response = await _mediator.Send(new CancelOrderCommand { OrderId = orderId} );
            if (response.Success)
            {
                _logger.LogInfo($"Order {response.OrderId} cancelled Succesfully.");

                return Ok(response);
            }

            _logger.LogError($"Failed to Cancel order: {response.Message}");
            return BadRequest(response.Message);

        }
    }


}
