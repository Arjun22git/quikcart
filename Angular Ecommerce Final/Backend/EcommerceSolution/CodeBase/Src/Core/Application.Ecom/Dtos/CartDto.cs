using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Ecom.Dtos
{
    public class CartDto
    {
        [JsonIgnore]
        public Guid CartId { get; set; }
        [JsonIgnore]
        public Guid CustomerId { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        public decimal TotalPrice { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }

}
