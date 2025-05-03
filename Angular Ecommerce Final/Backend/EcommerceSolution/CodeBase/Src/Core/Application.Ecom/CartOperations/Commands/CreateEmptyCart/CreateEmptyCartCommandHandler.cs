using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.CreateEmptyCart
{
    public class CreateEmptyCartCommandHandler : IRequestHandler<CreateEmptyCartCommand, CreateEmptyCartResponse>
    {
        private readonly IEcommerceDbContext _context; 
        private readonly ILoggerManager _logger;

        public CreateEmptyCartCommandHandler(IEcommerceDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CreateEmptyCartResponse> Handle(CreateEmptyCartCommand request, CancellationToken cancellationToken)
        {
            var cart = new Cart
            {
                CustomerId = request.CustomerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                
            };

            try
            {
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
                return new CreateEmptyCartResponse { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create an empty cart for customer {request.CustomerId}: {ex.Message}");
                return new CreateEmptyCartResponse { Success = false, Message = "Failed to create an empty cart." };
            }
        }
    }
}
