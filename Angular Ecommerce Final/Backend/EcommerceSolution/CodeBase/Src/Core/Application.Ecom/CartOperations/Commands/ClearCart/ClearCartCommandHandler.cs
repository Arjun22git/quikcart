using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, ClearCartResponse>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public ClearCartCommandHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ClearCartResponse> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Clearing cart for customer {request.CustomerId}");

            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);

            if (cart == null)
            {
                _logger.LogWarn($"Cart not found for customer {request.CustomerId}");
                return new ClearCartResponse { Success = false, Message = "Cart not found" };
            }

            _dbContext.CartItems.RemoveRange(cart.CartItems);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInfo($"Cart cleared for customer {request.CustomerId}");
            return new ClearCartResponse { Success = true, Message = "Cart Cleared" };
        }
    }
}
