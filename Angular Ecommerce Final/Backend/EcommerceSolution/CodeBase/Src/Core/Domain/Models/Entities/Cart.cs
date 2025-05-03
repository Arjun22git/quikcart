using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Cart
    {
        /// <summary>
        /// gets and sets CartId
        /// Primary Key Cart ID
        /// </summary>
        [Key]
        public Guid CartId { get; set; } 

        /// <summary>
        /// gets and sets CustomerId
        /// Customer ID as Foreign Key to User Table
        /// </summary>
        [Required]
        public Guid CustomerId { get; set; } 
        [ForeignKey(nameof(CustomerId))]

        /// <summary>
        ///  gets and sets Customer
        /// Navigation Property from Cart to Customer (1 to 1 mapping) 
        /// </summary>
        public User Customer { get; set; }

        /// <summary>
        ///  gets and sets Total cart value
        /// </summary>
        public decimal CartTotal { get; set; }

        /// <summary>
        ///  gets and sets CartItems
        /// Collection/List of items in Cart
        /// </summary>
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        /// <summary>
        ///  gets and sets CreatedAt
        /// Cart Creation Date -  Same as user Creation Date 
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// gets and sets UpdatedAt
        /// Last Cart Update  Date and Time
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

}
