using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Interfaces
{
    public interface IEcommerceDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Seller> Sellers { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Cart> Carts { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<Otp> Otps { get; set; }
        DbSet<Payment> Payments { get; set; }

        DbSet<Return> Returns {  get; set; }  
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
    }
}
