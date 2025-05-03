using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Product
    {
        /// <summary>
        /// gets and Sets ProductId
        /// </summary>
        [Key]
        public Guid ProductId { get; set; }


        /// <summary>
        /// gets and Sets Product Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }


        /// <summary>
        /// gets and Sets Description of Product
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// gets and Sets Category of Product
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Category { get; set; }

        /// <summary>
        /// gets and Sets Product Price
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// gets and Sets Quantity/Stock of Product
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// gets and Sets url of product image
        /// </summary>
        [Required]
        public string Imageurl { get; set; }


        /// <summary>
        /// gets and Sets IsDeleted 
        /// </summary>
        public bool IsDeleted { get; set; }


        /// <summary>
        /// gets and Sets SellerId
        /// Foreign key to Seller Table(many to 1 mapping)
        /// </summary>
        [Required]
        public Guid SellerId { get; set; } 
        [ForeignKey(nameof(SellerId))]

        /// <summary>
        /// gets and Sets Seller
        /// Navigation Property from Product to Seller table
        /// </summary>
        public Seller Seller { get; set; }

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
