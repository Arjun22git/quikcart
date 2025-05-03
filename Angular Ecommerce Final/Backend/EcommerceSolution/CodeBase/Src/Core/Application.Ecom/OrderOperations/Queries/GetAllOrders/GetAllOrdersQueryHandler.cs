using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using Application.Ecom.ProductOperations.Queries.GetAllProducts;
using Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public GetAllOrdersQueryHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _dbContext.Orders
                .Where(o => !o.IsCancelled) 
                .Include(o => o.Customer)  
                .Include(o => o.OrderItems)  
                    .ThenInclude(oi => oi.Product) 
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Customer.UserName,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    IsPaid = o.IsPaid,
                    OrderStatus = o.OrderStatus,
                    IsDelivered = o.IsDelivered,
                    ShippingAddress = o.ShippingAddress,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            _logger.LogInfo($"Retrieved {orders.Count} orders.");

            return orders;
        }
    }
}
