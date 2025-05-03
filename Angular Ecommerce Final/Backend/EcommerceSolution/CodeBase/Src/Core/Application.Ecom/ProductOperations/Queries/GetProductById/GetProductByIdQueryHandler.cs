using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.ProductOperations.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public GetProductByIdQueryHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Include(s => s.Seller)
                .Where(p => p.ProductId == request.ProductId && p.SellerId==p.Seller.SellerId)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Category = p.Category,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SellerId = p.SellerId,
                    SellerName = p.Seller.StoreName
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                _logger.LogWarn($"Product with ID {request.ProductId} not found.");
            }

            return product;
        }
    }

}
