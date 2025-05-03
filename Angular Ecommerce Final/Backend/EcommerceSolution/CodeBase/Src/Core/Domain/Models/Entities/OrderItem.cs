using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class OrderItem
    {
        /// <summary>
        /// gets and Sets OrderItemId
        /// PrimaryKey to identify Each Item in an Order
        /// </summary>
        [Key]
        public Guid OrderItemId { get; set; }

        /// <summary>
        /// gets and Sets OrderId
        /// Foreign key to Order Table using OrderID
        /// </summary>
        [Required]
        public Guid OrderId { get; set; } // Foreign Key to Order
        [ForeignKey(nameof(OrderId))]
        /// <summary>
        /// gets and Sets Order
        ///Navigation Property from OrderItem to Order
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// gets and Sets ProductId
        /// Foreign Key to Product Table to identify each product (many to 1)
        /// </summary>
        [Required]
        public Guid ProductId { get; set; } 
        [ForeignKey(nameof(ProductId))]

        /// <summary>
        /// gets and Sets Product
        /// Navigation Property to Product 
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// gets and Sets Quantity
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// gets and Sets UnitPrice of a Product
        /// Price of A unit
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// gets and Sets CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// gets and Sets UpdatedAt
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

}
