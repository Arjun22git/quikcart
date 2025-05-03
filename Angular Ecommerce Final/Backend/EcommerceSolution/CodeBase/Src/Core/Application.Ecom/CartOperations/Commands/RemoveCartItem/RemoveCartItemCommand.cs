using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.RemoveCartItem
{
    public class RemoveCartItemCommand : IRequest<RemoveCartItemResponse>
    {
        public Guid CartItemId { get; set; }
    }
}
