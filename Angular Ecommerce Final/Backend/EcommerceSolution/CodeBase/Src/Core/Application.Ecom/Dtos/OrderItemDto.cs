using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Ecom.Dtos
{
    public class OrderItemDto
    {

        public Guid ProductId { get; set; } // Foreign Key to Product

        [Required]
        public int Quantity { get; set; } // Quantity of the product in the order

        [JsonIgnore]
        public string ProductName { get; set; }
        
      

    }
}
