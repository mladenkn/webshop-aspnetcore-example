using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    public class WebShopDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItemDiscounted> BasketItemDiscountedEvents { get; set; }

        public WebShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BasketItemDiscounted>().HasKey(e => new {e.BasketItemId, e.DiscountId});
        }
    }
}
