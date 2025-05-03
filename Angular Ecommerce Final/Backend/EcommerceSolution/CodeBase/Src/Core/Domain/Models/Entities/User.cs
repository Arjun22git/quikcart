using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Entities
{
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// gets and sets Isverified 
        /// To check if the user is Verified
        /// </summary>
        public bool IsVerified { get; set; }


        /// <summary>
        /// gets and Sets CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// gets and Sets UpdatedAt
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// gets and Sets IsDeleted
        /// softDelete
        /// </summary>
        public bool IsDeleted {  get; set; }

        /// <summary>
        /// gets and Sets List Of Products
        /// Navigation Property to Products (1-many)
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// gets and Sets List Of Orders
        /// Navigation Property to Orders (1-many)
        /// </summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// gets and Sets Cart
        /// Navigation Property to Cart 
        /// </summary>
        public Cart Cart { get; set; }
        /// <summary>
        /// gets and Sets List Of Sellers
        /// Navigation Property to Sellers (1-many)
        /// </summary>
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();
    }

}
