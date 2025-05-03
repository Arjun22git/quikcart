using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Seller
    {
        /// <summary>
        /// gets and Sets SellerId
        /// Primary key to identify Each Seller
        /// </summary>
        [Key]
        public Guid SellerId { get; set; }

        /// <summary>
        /// gets and Sets UserId
        /// Foreign key to User Table
        /// </summary>
        [Required]
        public Guid UserId { get; set; } 
        [ForeignKey(nameof(UserId))]

        /// <summary>
        /// gets and Sets User
        /// Navigation Property to User
        /// </summary>
        public User User { get; set; }


        /// <summary>
        /// gets and Sets Storename
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string StoreName { get; set; }

        /// <summary>
        /// gets and Sets StoreDescription
        /// </summary>
        [MaxLength(500)]
        public string StoreDescription { get; set; }
        /// <summary>
        /// gets and Sets IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }


        /// <summary>
        /// gets and Sets CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// gets and Sets UpdatedAt
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// gets and Sets Products List
        /// List of Products of Each Seller
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

}
