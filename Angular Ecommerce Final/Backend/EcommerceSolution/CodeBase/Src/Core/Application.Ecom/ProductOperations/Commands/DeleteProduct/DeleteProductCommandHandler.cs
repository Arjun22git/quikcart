using Application.Ecom.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.ProductOperations.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponse>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public DeleteProductCommandHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                _logger.LogWarn($"Product with ID {request.ProductId} not found.");
                return new DeleteProductResponse
                {
                    Success = false,
                    Message = "Product not found."
                };
            }
            product.IsDeleted = true;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInfo($"Product deleted: {request.ProductId}");

            return new DeleteProductResponse
            {
                Success = true,
                Message = "Product deleted successfully."
            };
        }
    }

}
