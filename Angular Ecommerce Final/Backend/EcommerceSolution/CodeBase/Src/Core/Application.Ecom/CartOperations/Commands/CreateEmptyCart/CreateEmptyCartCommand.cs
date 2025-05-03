using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.CreateEmptyCart
{
    public class CreateEmptyCartCommand : IRequest<CreateEmptyCartResponse>
    {
        public Guid CustomerId { get; set; }
    }
}
