using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<UpdateCartItemResponse>
    {
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
