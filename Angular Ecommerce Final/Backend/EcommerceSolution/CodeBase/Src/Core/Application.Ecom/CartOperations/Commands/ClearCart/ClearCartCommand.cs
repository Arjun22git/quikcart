using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.ClearCart
{
    public class ClearCartCommand : IRequest<ClearCartResponse>
    {
        public Guid CustomerId { get; set; }
    }
}
