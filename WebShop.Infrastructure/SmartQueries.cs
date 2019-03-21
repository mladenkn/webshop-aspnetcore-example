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

        public async Task<IEnumerable<Discount>> GetDiscountsFor(Basket basket)
        {
            Expression<Func<Discount, bool>> basketContainsRequiredProductsInRequiredQuantity =
                d => d.RequiredProducts.All(rp =>
                    basket.Items.Count(bi => bi.ProductId == rp.ProductId) >= rp.RequiredQuantity);

            Expression<Func<Discount, bool>> basketContainsAnyTargetProducts =
                d => d.MicroDiscounts.All(md => basket.Items.Any(bi => bi.ProductId == md.TargetProductId));

            var r = await _db.Set<Discount>()
                .Include(d => d.RequiredProducts)
                .Include(d => d.MicroDiscounts)
                .Where(basketContainsRequiredProductsInRequiredQuantity)
                .Where(basketContainsAnyTargetProducts)
                .ToListAsync();

            return r;
        }
    }
}
