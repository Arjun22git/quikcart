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

namespace Application.Ecom.OrderOperations.Queries.TrackOrder
{
    public class TrackOrderQueryHandler : IRequestHandler<TrackOrderQuery, TrackOrderQueryResponse>
    {
        private readonly IEcommerceDbContext _context;
        private readonly ILoggerManager _logger;

        public TrackOrderQueryHandler(IEcommerceDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TrackOrderQueryResponse> Handle(TrackOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId, cancellationToken);

            if (order == null)
            {
                _logger.LogWarn($"Order with ID {request.OrderId} not found.");
                return new TrackOrderQueryResponse
                {
                    Success = false,
                    Message = $"Order with ID {request.OrderId} not found."
                };
            }
            var orderDto = new OrderDto                    //Dto Mapping
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                ShippingAddress = order.ShippingAddress,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems.Select(ci => new OrderItemDto
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };

            return new TrackOrderQueryResponse
            {
                Success = true,
                Order = orderDto,
                Message = "Order Fetched Successfully"
            };
        }
    }


}
