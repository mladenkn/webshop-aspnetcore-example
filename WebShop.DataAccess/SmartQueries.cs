using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    public class SmartQueries : ISmartQueries
    {
        private readonly WebShopDbContext _db;

        public SmartQueries(WebShopDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<BasketDiscount>> GetDiscountsFor(Basket basket)
        {
            Expression<Func<BasketDiscount, bool>> basketContainsRequiredProductsInMinimalRequiredQuantity =
                d => basket.Items.Count(bi => bi.ProductId == d.RequiredProductId) >= d.RequiredPerOneDiscounted;

            Expression<Func<BasketDiscount, bool>> basketContainsAnyTargetProducts =
                d => basket.Items.Any(bi => bi.ProductId == d.TargetProductId);

            var r = await _db.Set<BasketDiscount>()
                .Where(basketContainsRequiredProductsInMinimalRequiredQuantity)
                .Where(basketContainsAnyTargetProducts)
                .ToListAsync();

            return r;
        }
    }
}
