using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Ecom.OrderOperations.Queries.GetSellerOrders
{
    public class GetSellerOrdersQueryHandler : IRequestHandler<GetSellerOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public GetSellerOrdersQueryHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetSellerOrdersQuery request, CancellationToken cancellationToken)
        {

            var sellerorders = await _dbContext.Orders
                                     .Include(o => o.Customer)
                                     .Include(o => o.OrderItems)
                                         .ThenInclude(oi => oi.Product)
                    .Where(o => o.OrderItems.Any(oi => oi.Product.Seller.SellerId == request.SellerId))
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

            _logger.LogInfo($"Retrieved {sellerorders.Count} orders.");

            return sellerorders;
        }
    }
}
