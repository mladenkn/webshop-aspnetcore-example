using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    public class WebShopDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<BasketDiscount> Discounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }

        public WebShopDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
