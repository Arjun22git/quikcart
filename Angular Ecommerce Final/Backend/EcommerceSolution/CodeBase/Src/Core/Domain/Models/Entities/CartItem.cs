using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class CartItem
    {
        /// <summary>
        /// gets and sets Cart Item Id
        /// Cart item Id to identify each cart item - primary key
        /// </summary>
        [Key]
        public Guid CartItemId { get; set; } 

        /// <summary>
        /// gets and sets CartId
        /// Cart Id to identify which cart the item belongs to - Foreign key to Cart table  (1-1 mapping)
        /// </summary>
        [Required]
        public Guid CartId { get; set; } 
        [ForeignKey(nameof(CartId))]

        /// <summary>
        /// gets and sets Cart
        /// navgation property to Cart Table
        /// </summary>
        public Cart Cart { get; set; }

        /// <summary>
        /// gets and sets productId
        /// Product Id to identify each Product in the Cart item list (1-many mapping)
        /// </summary>
        [Required]
        public Guid ProductId { get; set; } 
        [ForeignKey(nameof(ProductId))]

        /// <summary>
        /// gets and sets Product
        /// Navigation property to Product table
        /// </summary>
        public Product Product { get; set; }


        /// <summary>
        /// gets and sets Quantity
        /// Quantity of the product/item in the cart
        /// </summary>

        [Required]
        public int Quantity { get; set; }


        /// <summary>
        /// gets and sets Price
        /// Quantity of the product/item in the cart
        /// </summary>

        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// sets rowversion
        /// Rowversion for concurrency check
        /// </summary>
        [Timestamp]
        public byte[] Version { get; set; }
       
        /// <summary>
        /// gets and sets CreatedAt
        ///Cart item creation date- date on which the item was added to cart 
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// gets and sets UpdatedAt
        ///Cart item updation date
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }


}
