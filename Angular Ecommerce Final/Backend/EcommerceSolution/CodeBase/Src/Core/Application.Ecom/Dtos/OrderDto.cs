using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Application.Ecom.Dtos
{
    public class OrderDto
    {

        [JsonIgnore]
        public Guid OrderId { get; set; } // Primary Key  
        [JsonIgnore]
        public Guid CustomerId { get; set; }
        [JsonIgnore]
        public User Customer { get; set; }
        [JsonIgnore]
        public Guid PaymentId { get; set; }
        public string CustomerName { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        
        public string OrderStatus { get; set; }
        public DateOnly DeliveryDate { get; set; }

        public string ShippingAddress { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }

       
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public bool IsPaid { get; set; }
        [JsonIgnore]
        public bool IsDelivered { get; set; }
    }
}
