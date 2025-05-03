using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Ecom.ProductOperations.Queries.GetProductsBySeller
{
    public class GetSellerProductsQueryHandler : IRequestHandler<GetSellerProductsQuery, List<ProductDto>>
    {
        private readonly IEcommerceDbContext _dbContext;

        public GetSellerProductsQueryHandler(IEcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductDto>> Handle(GetSellerProductsQuery request, CancellationToken cancellationToken)
        {
            
            if (request.SellerId == Guid.Empty)
            {
                throw new ArgumentException("SellerId cannot be empty.", nameof(request.SellerId));
            }

            var sellerExists = await _dbContext.Sellers.AnyAsync(s => s.SellerId == request.SellerId, cancellationToken);
            if (!sellerExists)
            {
                throw new KeyNotFoundException("Seller not found.");
            }

            try
            {
                var products = await _dbContext.Products
                    .Where(p => p.SellerId == request.SellerId)
                    .Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Price = p.Price,
                        Description= p.Description,
                        Category = p.Category,
                        Quantity = p.Quantity,
                        ImageUrl = p.Imageurl
                    })
                    .ToListAsync(cancellationToken);

                return products;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving products.", ex);
            }
        }

    }
}
