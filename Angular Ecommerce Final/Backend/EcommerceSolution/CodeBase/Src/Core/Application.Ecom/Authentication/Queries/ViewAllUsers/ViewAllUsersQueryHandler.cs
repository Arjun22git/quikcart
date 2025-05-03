using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Queries.ViewAllUsers
{
    public class ViewAllUsersQueryHandler : IRequestHandler<ViewAllUsersQuery, IEnumerable<User>>
    {
        private readonly IEcommerceDbContext _context; 

        public ViewAllUsersQueryHandler(IEcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> Handle(ViewAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(user => !user.IsDeleted) 
                .ToListAsync(cancellationToken);
        }
    }
}
