using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using Application.Ecom.OrderOperations.Queries.TrackOrder;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery,OrderDto>
    {
        private readonly IEcommerceDbContext _context;
        private readonly ILoggerManager _logger;

        public GetOrderByIdQueryHandler(IEcommerceDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(c=>c.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId, cancellationToken);

            if (order == null)
            {
                _logger.LogWarn($"Order with ID {request.OrderId} not found.");
                return null;
            }
            var orderDto = new OrderDto                    //Dto Mapping
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.UserName,
                ShippingAddress = order.ShippingAddress,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                OrderItems = order.OrderItems.Select(ci => new OrderItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName=ci.Product.Name,
                    Quantity = ci.Quantity,
                }).ToList()
            };

            return orderDto;
        }
    }
}
