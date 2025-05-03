using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Ecom.Dtos
{
    public class CartItemDto
    {
        [JsonIgnore]
        public Guid CartId { get; set; }
        public Guid CartItemId { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }  

        public string imgurl { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
       
    }

}
