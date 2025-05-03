using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public enum PaymentStatus
    {
        Failed,
        Success,     
        Pending
    }
    public class Payment
    {
        /// <summary>
        /// gets and Sets PaymentId
        /// Primary Key to identify each Payment
        /// </summary>
        [Key]
        public Guid PaymentId { get; set; }

        /// <summary>
        /// gets and Sets OrderId
        /// Foreign key to Order Table (1-1 mapping)
        /// </summary>
        [Required]
        public Guid OrderId { get; set; } // Foreign Key to Order
        [ForeignKey(nameof(OrderId))]
        /// <summary>
        /// gets and Sets Order
        /// Navigation Property From Payment to Order 
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// gets and Sets Amount
        /// Stores the Amount Paid in the transaction
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// gets and Sets PaymentMethod
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// gets and Sets Payment Date
        /// </summary>
        [Required]
        public DateTime PaymentDate { get; set; }


        /// <summary>
        /// gets and Sets Payment Status
        /// Status - Pending,Success,Failure
        /// </summary>
        [Required]
        [MaxLength(50)]
        public PaymentStatus PaymentStatus { get; set; } // Status of the Payment (e.g., Completed, Pending)


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
    }

}
