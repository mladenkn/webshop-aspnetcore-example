using Microsoft.EntityFrameworkCore;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Infrastructure
{
    public class WebShopDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }

        public WebShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BasketItem>().Ignore(it => it.Discounts);
            modelBuilder.Entity<BasketItem>().Ignore(it => it.Price);
            modelBuilder.Entity<Basket>().Ignore(it => it.TotalPrice);
        }
    }
}
