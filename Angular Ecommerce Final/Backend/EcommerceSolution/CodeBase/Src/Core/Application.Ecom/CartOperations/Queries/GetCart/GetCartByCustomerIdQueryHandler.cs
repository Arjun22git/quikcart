using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Queries.GetCart
{
    public class GetCartByCustomerIdQueryHandler : IRequestHandler<GetCartByCustomerIdQuery, CartDto>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public GetCartByCustomerIdQueryHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CartDto> Handle(GetCartByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Retrieving cart for customer {request.CustomerId}");

            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);

            if (cart == null)
            {
                _logger.LogWarn($"Cart not found for customer {request.CustomerId}");
                return null;
            }

            cart.CartTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);
            _dbContext.Carts.Update(cart);
            // Map to DTO
            var cartDto = new CartDto
            {
                CartId = cart.CartId,
                CustomerId = cart.CustomerId,
                TotalPrice=cart.CartTotal,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                    imgurl = ci.Product.Imageurl,
                }).ToList()
            };

            _logger.LogInfo($"Cart retrieved for customer {request.CustomerId}");
            return cartDto;
        }
    }
}
