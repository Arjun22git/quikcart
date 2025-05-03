using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Order
    {
        /// <summary>
        /// gets and sets OrderId
        ///Primary key - order Id
        /// </summary>
        [Key]
        public Guid OrderId { get; set; }

        /// <summary>
        /// gets and Sets CustomerId
        ///Customer Id as Foreign Key to user Table
        /// </summary>
        [Required]
        public Guid CustomerId { get; set; } 
        [ForeignKey(nameof(CustomerId))]

        /// <summary>
        /// gets and Sets Customer
        ///Navigation Proeperty to Customer table
        /// </summary>
        public User Customer { get; set; }

        /// <summary>
        /// gets and Sets OrderDate
        ///Order Date
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// gets and Sets TotalAmount
        ///Store Total Order Value
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// gets and Sets Shipping Address
        ///Address to be delivered
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// gets and Sets OrderItems
        ///List of Items in the Order
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        /// <summary>
        /// gets and Sets Payment Id
        ///Payment Id Foreign key to Table Payment wiht property PaymentId
        /// </summary>
        public Guid? PaymentId { get; set; } 
        [ForeignKey(nameof(PaymentId))]

        /// <summary>
        /// gets and Sets Payment
        ///Navigation Proeperty to Payments table
        /// </summary>
        public Payment Payments { get; set; } // Navigation for Payment

        /// <summary>
        /// gets and Sets OrderStatus
        ///Set the status of the Order
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; }
        /// <summary>
        /// gets and Sets DeliveryDate
        ///Expected Date of delivery
        /// </summary>
        public DateOnly DeliveryDate { get; set; }

        /// <summary>
        /// gets and sets returnId 
        /// Foreign Key to table Return
        /// </summary>
        public Guid? ReturnId { get; set; }
        [ForeignKey(nameof(ReturnId))]
        /// <summary>
        /// gets and sets return
        /// </summary>
        public Return Return { get; set; }

        /// <summary>
        /// gets and Sets CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// gets and Sets UpdatedAt
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// gets and Sets IsPaid
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// gets and Sets Isdelivered
        /// </summary>
        public bool IsDelivered { get; set; }

        /// <summary>
        /// gets and Sets IsCancelled
        /// </summary>
        public bool IsCancelled { get; set; }
    }

}
