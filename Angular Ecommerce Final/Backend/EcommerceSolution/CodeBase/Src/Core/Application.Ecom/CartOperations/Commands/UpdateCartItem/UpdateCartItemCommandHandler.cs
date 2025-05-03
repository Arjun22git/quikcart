using Application.Ecom.CartOperations.Commands.AddCartItem;
using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Ecom.CartOperations.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, UpdateCartItemResponse>
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public UpdateCartItemCommandHandler(IEcommerceDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<UpdateCartItemResponse> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
            {
                return new UpdateCartItemResponse { Success = false, Message = $"Invalid Quantity! Minimum Quantity is 1!" };
            }
            if (request.Quantity >= 20)
            {
                return new UpdateCartItemResponse { Success = false, Message = $"Invalid Quantity! Maximum Quantity for a user is 20!" };
            }
            _logger.LogInfo($"Updating cart item {request.CartItemId} with new quantity {request.Quantity}");

            var cartItem = await _dbContext.CartItems
                .SingleOrDefaultAsync(ci => ci.CartItemId == request.CartItemId, cancellationToken);

            if (cartItem == null)
            {
                _logger.LogWarn($"Cart item {request.CartItemId} not found");
                return new UpdateCartItemResponse { Success = false, Message = "Cart item not found" };
            }

            cartItem.Quantity = request.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInfo($"Cart item {request.CartItemId} updated with new quantity {request.Quantity}");
            return new UpdateCartItemResponse { Success = true,Message= $"Cart item {request.CartItemId} updated with new quantity {request.Quantity}" };
        }
    }
}
