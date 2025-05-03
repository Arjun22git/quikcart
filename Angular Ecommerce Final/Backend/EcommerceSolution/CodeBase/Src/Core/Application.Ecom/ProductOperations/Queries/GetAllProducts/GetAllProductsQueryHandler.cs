using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.ProductOperations.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedProductResponse>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public GetAllProductsQueryHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PaginatedProductResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)

        {
            var productsQuery = _dbContext.Products
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Category = p.Category,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    ImageUrl = p.Imageurl,
                    SellerId = p.SellerId,
                    SellerName = p.Seller.StoreName
                });

            var totalProducts = await productsQuery.CountAsync(cancellationToken);

            var products = await productsQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            _logger.LogInfo($"Retrieved {products.Count} products for page {request.PageNumber}.");

            return new PaginatedProductResponse
            {
                Products = products,
                TotalCount = totalProducts,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }

}
