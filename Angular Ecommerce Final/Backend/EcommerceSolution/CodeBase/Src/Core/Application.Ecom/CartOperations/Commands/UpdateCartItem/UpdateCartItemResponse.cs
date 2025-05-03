using Application.Ecom.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.UpdateCartItem
{
    public class UpdateCartItemResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public CartItemDto CartItem { get; set; }
    }
}
