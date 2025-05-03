using Application.Ecom.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, CommandResponse>
    {
        private readonly IEcommerceDbContext _context;
        private readonly ILoggerManager _logger;

        public UpdateOrderStatusCommandHandler(IEcommerceDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CommandResponse> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);

            if (order == null)
            {
                _logger.LogWarn($"Order with ID {request.OrderId} not found.");
                return new CommandResponse
                {
                    Success = false,
                    Message = $"Order with ID {request.OrderId} not found."
                };
            }

            // Update OrderStatus if provided
            if (!string.IsNullOrEmpty(request.OrderStatus))
            {
                order.OrderStatus = request.OrderStatus;
            }

            // Update DeliveryDate if provided
            if (request.IsDelivered)
            {
                order.IsDelivered = true;
                order.DeliveryDate = request.DeliveryDate; // Use provided date or default to now
            }          

            order.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInfo($"Order {request.OrderId} status updated to {order.OrderStatus}.");
                return new CommandResponse { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update order status: {ex.Message}");
                return new CommandResponse { Success = false, Message = "Failed to update order status." };
            }
        }
    }

}
