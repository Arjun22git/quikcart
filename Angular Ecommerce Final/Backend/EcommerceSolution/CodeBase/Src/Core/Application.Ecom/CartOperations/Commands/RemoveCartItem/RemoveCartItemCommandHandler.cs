using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, RemoveCartItemResponse>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public RemoveCartItemCommandHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<RemoveCartItemResponse> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Removing cart item {request.CartItemId}");

            var cartItem = await _dbContext.CartItems
                .SingleOrDefaultAsync(ci => ci.CartItemId == request.CartItemId, cancellationToken);

            if (cartItem == null)
            {
                _logger.LogWarn($"Cart item {request.CartItemId} not found");
                return new RemoveCartItemResponse { Success = false, Message = "Cart item not found" };
            }

            _dbContext.CartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInfo($"Cart item {request.CartItemId} removed");
            return new RemoveCartItemResponse { Success = true,Message = "Item Removed from Cart Successfully" };
        }
    }
}
