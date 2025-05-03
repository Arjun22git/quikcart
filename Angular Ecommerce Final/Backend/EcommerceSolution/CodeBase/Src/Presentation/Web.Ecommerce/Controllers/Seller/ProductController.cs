using Application.Ecom.Interfaces;
using Application.Ecom.ProductOperations.Commands.CreateProduct;
using Application.Ecom.ProductOperations.Commands.DeleteProduct;
using Application.Ecom.ProductOperations.Commands.UpdateProduct;
using Application.Ecom.ProductOperations.Queries.GetAllProducts;
using Application.Ecom.ProductOperations.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.Entities;

namespace Web.Ecommerce.Controllers.Seller
{
    /// <summary>
    /// Product Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerManager _logger;
        private readonly ITokenAuthenticationService _tokenAuthenticationService;

        public ProductController(IMediator mediator, ILoggerManager logger, ITokenAuthenticationService tokenAuthenticationService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenAuthenticationService = tokenAuthenticationService;
        }
       
        /// <summary>
        /// Add a new Product to the Seller's Store - Seller Only
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("AddNewProduct")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarn("CreateProduct request model is invalid.");
                return BadRequest(ModelState);
            }

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                _logger.LogInfo($"Product created successfully with ID: {response.Product.ProductId}");
                return CreatedAtAction(nameof(GetProductById), new { id = response.Product.ProductId }, response.Product);
            }

            _logger.LogError($"Failed to create product: {response.Message}");
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Update the details of a product - Seller Only
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("UpdateProductDetails")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Success)
            {
                _logger.LogInfo($"Product updated successfully!");
                return NoContent();
            }

            _logger.LogError($"Failed to update product: {response.Message}");
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Delete a product by its Id - Seller Only
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var response = await _mediator.Send(new DeleteProductCommand { ProductId = id });
            if (response.Success)
            {
                _logger.LogInfo($"Product deleted successfully with ID: {id}");
                return NoContent();
            }

            _logger.LogError($"Failed to delete product: {response.Message}");
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Search a particular Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProductById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var response = await _mediator.Send(new GetProductByIdQuery { ProductId = id });
            if (response != null)
            {
                _logger.LogInfo($"Retrieved product with ID: {id}");
                return Ok(response);
            }

            _logger.LogWarn($"Product with ID {id} not found.");
            return NotFound($"Product with ID {id} not found.");
        }

        /// <summary>
        /// Get All the Products of the Seller
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllProducts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _mediator.Send(new GetAllProductsQuery());
            _logger.LogInfo($"Retrieved {response.PageSize} products.");
            return Ok(response);
        }
    }

}
