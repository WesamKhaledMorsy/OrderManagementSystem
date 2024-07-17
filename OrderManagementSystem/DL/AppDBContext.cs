using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.DL
{
    public class AppDBContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole<int>>().ToTable("ApplicationRoles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("ApplicationUserClaims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("ApplicationUserLogins");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("ApplicationUserRoles");
            modelBuilder.Entity<IdentityUser<int>>().ToTable("ApplicationUsers");

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.OrderItemId).ValueGeneratedOnAdd();
            });
        }
    }
}
