using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{

    public class EcommerceDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IEcommerceDbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Return> Returns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Seller entity
            modelBuilder.Entity<Seller>()
                .HasKey(s => s.SellerId);
            modelBuilder.Entity<Seller>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sellers)
                .HasForeignKey(s => s.UserId);

            // Configure Product entity
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(8,2)");
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SellerId);

            // Configure Cart entity
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.CartId);
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.CustomerId);

            // Configure CartItem entity
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => ci.CartItemId);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);
            //modelBuilder.Entity<CartItem>()
            //    .Property(p => p.RowVersion)          //concurrency check
            //    .IsRowVersion();
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete from Product to CartItem

            // Configure Order entity
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payments)
                .WithMany()
                .HasForeignKey(o => o.PaymentId);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Return)
                .WithMany()
                .HasForeignKey(o => o.ReturnId)
                .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict to avoid multiple cascade paths



            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // OrderItems will be deleted when Order is deleted
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete from Product to OrderItem

            // Configure Payment entity
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.PaymentId);
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payments)
                .HasForeignKey<Payment>(p => p.OrderId);

            // Configure Return entity
            modelBuilder.Entity<Return>()
                .HasKey(r => r.ReturnId);
            modelBuilder.Entity<Return>()
                .HasOne(r => r.Order)
                .WithOne(o => o.Return)
                .HasForeignKey<Return>(r => r.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Return>()
                .HasOne(r => r.Payment)
                .WithMany()
                .HasForeignKey(r => r.PaymentId)
                .OnDelete(DeleteBehavior.SetNull); 
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}
