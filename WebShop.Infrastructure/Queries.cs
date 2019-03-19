using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    public class Queries : IQueries
    {
        private readonly WebShopDbContext _db;

        public Queries(WebShopDbContext db)
        {
            _db = db;
        }

        public Task<Basket> GetBasketWithItems(int basketId)
        {
            return _db.Baskets.Where(b => b.Id == basketId).Include(b => b.Items).FirstOrDefaultAsync();
        }

        public Task<Basket> GetUsersBasketWithItems(string userId)
        {
            return _db.Baskets.FirstOrDefaultAsync(b => b.UserId == userId);
        }
    }
}
