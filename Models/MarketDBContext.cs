using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Registration;
using Models.Tables;

namespace Models
{
    public class MarketDBContext : IdentityDbContext
    {
        public MarketDBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(o => o.Order)
                .HasForeignKey("Customer_Id");

            builder.Entity<OrderItem>()
               .HasOne(o => o.Order)
               .WithMany(o => o.OrderItems)
               .HasForeignKey("Order_Id");

            builder.Entity<OrderItem>()
               .HasOne(o => o.Item)
               .WithMany(i => i.OrderItems)
               .HasForeignKey(o => o.Item_Id);

            builder.Entity<AppUser>()
               .HasOne(a => a.Customer)
               .WithOne(o => o.AppUser)
               .HasForeignKey<AppUser>("Customer_Id");

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Manager", NormalizedName = "Manager" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "User" });
        }


        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Item> Items { get; set; }
    }

}
