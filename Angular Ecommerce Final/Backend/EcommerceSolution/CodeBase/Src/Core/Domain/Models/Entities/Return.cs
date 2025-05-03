using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Return
    {
        /// <summary>
        /// gets and sets ReturnId
        /// Return ID as Primary Key
        /// </summary>
        public Guid ReturnId { get; set; }

        /// <summary>
        /// Gets and Sets the Reason 
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// gets and Sets OrderId
        /// Foreign key to Order Table (1-1 mapping)
        /// </summary>
        [Required]
        public Guid OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

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
        public Payment Payment { get; set; }

        /// <summary>
        /// gets and Sets CreatedAt
        /// </summary>
        public DateOnly PickupDate { get; set; }

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
