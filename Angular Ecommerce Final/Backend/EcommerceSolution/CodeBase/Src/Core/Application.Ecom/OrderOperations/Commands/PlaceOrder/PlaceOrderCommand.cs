using Application.Ecom.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<PlaceOrderCommandResponse>
    {
        public Guid CustomerId { get; set; }

        [MinLength(1, ErrorMessage = "At least one order item is required.")]
        public List<CartItemDto> OrderItems { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        public string ShippingAddress { get; set; }
    }
}
