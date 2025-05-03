using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.AddCartItem
{
    public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, AddCartItemResponse>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public AddCartItemCommandHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<AddCartItemResponse> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
            {
                return new AddCartItemResponse { Success = false, Message = $"Invalid Quantity! Minimum Quantity is 1!" };
            }
            if (request.Quantity >= 20)
            {
                return new AddCartItemResponse { Success = false, Message = $"Invalid Quantity! Maximum Quantity for a user is 20!" };
            }

            _logger.LogInfo($"Adding product {request.ProductId} to cart for customer {request.CustomerId}");

            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .ThenInclude(c=>c.Product)
                .SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);

            if (cart == null)
            {
                _logger.LogWarn($"Cart not found for customer {request.CustomerId}");
                return new AddCartItemResponse { Success = false, Message = "Cart not found" };
            }

            var existingCartItem = cart.CartItems.SingleOrDefault(ci => ci.ProductId == request.ProductId);
            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity + request.Quantity >= existingCartItem.Product.Quantity || (existingCartItem.Quantity >= 3) )
                {
                    return new AddCartItemResponse
                    {
                        Message = "Quantity Exceeded",
                        Success = false
                    };

                }
                existingCartItem.Quantity += request.Quantity;
                existingCartItem.UpdatedAt = DateTime.UtcNow;
                _dbContext.CartItems.Update(existingCartItem);
            }
            else
            {
                var product = await _dbContext.Products.SingleOrDefaultAsync(x => x.ProductId == request.ProductId);
                var cartItem = new CartItem
                {
                    CartItemId = Guid.NewGuid(),
                    CartId = cart.CartId,
                    ProductId = product.ProductId,
                    Price = product.Price, // Set unit price
                    Quantity = request.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _dbContext.CartItems.Add(cartItem);

                //var cartItemDto = CartItemMapper.ToDto(cartItem);
                // Calculate the new CartTotal
                cart.CartTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);
            }

            try
            {
                     await _dbContext.SaveChangesAsync();
                    _logger.LogInfo($"Product {request.ProductId} added to cart for customer {request.CustomerId}");
                    return new AddCartItemResponse { Success = true, Message= $"Item Added To Cart Successfully" };

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogInfo($"Product {request.ProductId} could not be added to cart for customer {request.CustomerId}: {ex.Message}");
                return new AddCartItemResponse { Success = false, Message=$"Database Concurrency Exception : {ex.Message}" };
            }
            
        
            
        }
    }
}
