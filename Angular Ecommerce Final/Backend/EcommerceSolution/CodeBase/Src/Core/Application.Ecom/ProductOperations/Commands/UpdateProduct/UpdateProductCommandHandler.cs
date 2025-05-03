using Application.Ecom.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.ProductOperations.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public UpdateProductCommandHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                _logger.LogWarn($"Product with ID {request.ProductId} not found.");
                return new UpdateProductResponse
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Category = request.Category;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInfo($"Product updated: {request.ProductId}");

            return new UpdateProductResponse
            {
                Success = true,
                Message = "Product updated successfully."
            };
        }
    }

}
