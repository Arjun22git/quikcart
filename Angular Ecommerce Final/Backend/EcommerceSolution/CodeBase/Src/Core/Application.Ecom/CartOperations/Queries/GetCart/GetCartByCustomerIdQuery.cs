using Application.Ecom.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Queries.GetCart
{
    public class GetCartByCustomerIdQuery : IRequest<CartDto>
    {
        public Guid CustomerId { get; set; }
    }
}
