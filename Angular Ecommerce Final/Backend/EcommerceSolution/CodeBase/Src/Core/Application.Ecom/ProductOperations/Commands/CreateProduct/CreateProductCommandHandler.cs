using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.ProductOperations.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;
 
        public CreateProductCommandHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var seller = _dbContext.Sellers.FirstOrDefault(x=>x.SellerId == request.SellerId);
            
            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                Imageurl = request.Imageurl,
                Quantity = request.Quantity,
                SellerId = request.SellerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInfo($"Product created: {request.Name}");

            return new CreateProductResponse
            {
                Success = true,
                Message = "Product created successfully.",
                Product = new ProductDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Category = product.Category,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ImageUrl = product.Imageurl,
                    SellerId = product.SellerId,
                    SellerName =product.Seller.StoreName
                }
            };
        }
    }
}
