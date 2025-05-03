using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Ecom.OrderOperations.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, CancelOrderResponse>
    {

        private readonly IEcommerceDbContext _context;
        private readonly ILoggerManager _logger;

        public CancelOrderCommandHandler(IEcommerceDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CancelOrderResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            // Fetch the order with its items and related products
            var order = await _context.Orders
                .Include(c => c.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId, cancellationToken);

            // Check if the order exists
            if (order == null)
            {
                _logger.LogWarn($"Order with ID {request.OrderId} not found.");
                return new CancelOrderResponse
                {
                    Success = false,
                    Message = "Order Not Found"
                };
            }

            // Check if the order is already cancelled
            if (order.IsCancelled)
            {
                _logger.LogWarn($"Order with ID {request.OrderId} is already cancelled.");
                return new CancelOrderResponse
                {
                    Success = false,
                    Message = "Order is already cancelled."
                };
            }

            // Restore product quantities
            foreach (var orderItem in order.OrderItems) 
            {
                var product = await _context.Products.FindAsync(orderItem.ProductId);
                if (product != null)
                {
                    product.Quantity += orderItem.Quantity; // Restore the quantity
                    _context.Products.Update(product);
                }
            }

            // Mark the order as cancelled
            order.OrderStatus = "Cancelled";
            order.IsCancelled = true;
            await _context.SaveChangesAsync(cancellationToken);

            return new CancelOrderResponse
            {
                Message = "Your order has been cancelled successfully.",
                OrderId = order.OrderId,
                Success = true
            };
        }

    }
}
