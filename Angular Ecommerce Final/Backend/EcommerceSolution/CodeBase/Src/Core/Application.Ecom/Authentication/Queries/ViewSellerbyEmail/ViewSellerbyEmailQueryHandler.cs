using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Ecom.Authentication.Queries.ViewSellerbyEmail
{
    public class ViewSellerbyEmailQueryHandler : IRequestHandler<ViewSellerbyEmailQuery, object>
    {
        private readonly IEcommerceDbContext _context;
        private readonly ILoggerManager _loggerManager;
        private readonly UserManager<User> _userManager;

        public ViewSellerbyEmailQueryHandler(IEcommerceDbContext context, ILoggerManager loggerManager, UserManager<User> userManager)
        {
            _context = context;
            _loggerManager = loggerManager;
            _userManager = userManager;
        }

        public async Task<object> Handle(ViewSellerbyEmailQuery request, CancellationToken cancellationToken)
        {
           
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _loggerManager.LogError($"User with email {request.Email} not found.");
                throw new KeyNotFoundException($"User with email {request.Email} not found.");
            }

           
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Seller"))
            {
                _loggerManager.LogError($"User {request.Email} is not a seller.");
                throw new UnauthorizedAccessException($"User {request.Email} is not a seller.");
            }

            
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserId == user.Id, cancellationToken);

            if (seller == null)
            {
                _loggerManager.LogError($"Seller information not found for user {request.Email}.");
                throw new KeyNotFoundException($"Seller information not found for user {request.Email}.");
            }


            return new
            {
                sellerId = seller.SellerId,
                email = request.Email
            };
        }
    }


}
