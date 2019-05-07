using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.DataAccess
{
    public class Queries : IQueries
    {
        private readonly WebShopDbContext _db;

        public Queries(WebShopDbContext db)
        {
            _db = db;
        }

        public Task<Basket> GetUsersBasket(string userId, Action<IncludesBuilder<Basket>> includeProps)
        {
            return _db.Baskets
                .Include(includeProps)
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public Task<BasketItem> GetBasketItem(int id) => _db.BasketItems.FirstOrDefaultAsync(i => i.Id == id);
    }
}
