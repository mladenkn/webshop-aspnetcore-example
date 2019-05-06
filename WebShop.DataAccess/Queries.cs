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

        public Task<Basket> GetUsersBasketWithItemsAndDiscounts(string userId)
        {
            return _db.Baskets
                .Include(b => b.Items)
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public Task<Basket> GetUsersBasket(string userId)
        {
            return _db.Baskets
                .Where(b => b.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
