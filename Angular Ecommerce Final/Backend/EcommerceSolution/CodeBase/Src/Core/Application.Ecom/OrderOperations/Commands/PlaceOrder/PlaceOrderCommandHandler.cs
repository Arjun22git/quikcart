using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using Application.Ecom.OrderOperations.Mapper;
using Domain.Models.Entities;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Ecom.OrderOperations.Commands.PlaceOrder
{

    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderCommandResponse>
    {
        private readonly IEcommerceDbContext _context;
        private readonly ILoggerManager _logger;

        public PlaceOrderCommandHandler(IEcommerceDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PlaceOrderCommandResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            if (request.OrderItems == null || !request.OrderItems.Any())
            {
                return new PlaceOrderCommandResponse
                {
                    Success = false,
                    Message = "At least one cart item is required."
                };
            }
            var cart = await _context.Carts
        .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);

            if (cart == null)
            {
                return new PlaceOrderCommandResponse
                {
                    Success = false,
                    Message = "Cart not found for the given Customer ID."
                };
            }

            // Now fetch CartItems using CartId
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cart.CartId)
                .ToListAsync(cancellationToken);

            // Map CartItems to OrderItems
            var orderDto = MapOrder.MapCartToOrder(request.CustomerId, cartItems.Select(ci => new CartItemDto
            {
                CartItemId = ci.CartItemId,
                ProductId = ci.ProductId, 
                Quantity = ci.Quantity,
                Price = ci.Price,
                ProductName = ci.Product?.Name,
                imgurl = ci.Product?.Imageurl,
                CreatedAt = ci.CreatedAt,
                UpdatedAt = ci.UpdatedAt
            }).ToList(), request.ShippingAddress);

            // Create the Order
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                DeliveryDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10),
                OrderStatus = "Payment Pending",
                TotalAmount = await CalculateTotalAmount(orderDto.OrderItems),
                ShippingAddress = orderDto.ShippingAddress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Create OrderItems
            foreach (var itemDto in orderDto.OrderItems)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);

                if (product == null || product.Quantity < itemDto.Quantity)
                {
                    return new PlaceOrderCommandResponse
                    {
                        Success = false,
                        Message = $"Product {itemDto.ProductId} is not available or insufficient stock."
                    };
                }

                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.OrderItems.Add(orderItem);

                // Reduce product quantity
                await _context.ExecuteSqlRawAsync
                (
                    "EXEC ReduceProductQuantity @ProductId, @Quantity",
                    new SqlParameter("@ProductId", itemDto.ProductId),
                    new SqlParameter("@Quantity", itemDto.Quantity)
                );
            }

            // Add the order to the context
            _context.Orders.Add(order);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInfo($"Order {order.OrderId} placed successfully. Please process payment via the ProcessPayment endpoint.");

                return new PlaceOrderCommandResponse
                {
                    Success = true,
                    OrderId = order.OrderId,
                    Message = "Order placed successfully. Please process the payment to complete the order."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to place order: {ex.Message}");
                return new PlaceOrderCommandResponse
                {
                    Success = false,
                    Message = "Failed to place order."
                };
            }
        }

        private async Task<decimal> CalculateTotalAmount(List<OrderItemDto> orderItems)
        {
            decimal totalAmount = 0m;

            foreach (var item in orderItems)
            {
                var price = await GetProductPrice(item.ProductId);
                totalAmount += price * item.Quantity;
            }

            return totalAmount;
        }

        private async Task<decimal> GetProductPrice(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }
            return product.Price;
        }
    }
}
