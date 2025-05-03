using Application.Ecom.Authentication.Commands.RegisterCustomer;
using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Queries.ViewCustomerbyEmail
{
    public class ViewCustomerQueryHandler : IRequestHandler<ViewCustomerQuery, ViewCustomerQueryResponse>
    {   
        private readonly IEcommerceDbContext _context;
        private readonly ILoggerManager _loggerManager;
        private readonly UserManager<User> _userManager;
        public ViewCustomerQueryHandler(IEcommerceDbContext context, ILoggerManager loggerManager,UserManager<User> userManager)
        {
            _context = context;
            _loggerManager = loggerManager;
            _userManager = userManager;
        }


        public async Task<ViewCustomerQueryResponse> Handle (ViewCustomerQuery request,CancellationToken cancellationToken)
        {
            var customer = await _userManager.FindByEmailAsync(request.Email);
            if (customer == null)
            {
                return null;
            }

            return new ViewCustomerQueryResponse
            {
                CustomerId = customer.Id,
                CustomerName = customer.UserName,
                JoinDate = customer.CreatedAt,
                IsVerified = customer.IsVerified,
                
            };

        }
    }
}
