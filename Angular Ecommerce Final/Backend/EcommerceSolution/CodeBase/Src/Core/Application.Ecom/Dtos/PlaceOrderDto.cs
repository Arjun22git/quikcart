using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Ecom.Dtos
{
    public class PlaceOrderDto
    {
        [Required]
        public Guid CustomerId { get; set; } // ID of the customer placing the order

        [Required]
        public List<OrderItemDto> OrderItems { get; set; } // List of order items

        public string ShippingAddress { get; set; } // Shipping address for the order

    }
}
