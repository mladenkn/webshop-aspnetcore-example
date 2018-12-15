using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.DataAccess
{
    public class SmartQueries : ISmartQueries
    {
        private readonly WebShopDbContext _db;

        public SmartQueries(WebShopDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyCollection<Discount>> GetDiscountsFor(Basket basket)
        {
            Expression<Func<Discount, bool>> basketContainsProductInRequiredQuantity =
                d => basket.Items.Count(bi => bi.ProductId == d.RequiredProductId) >= d.RequiredProductRequiredQuantity;

            Expression<Func<Discount, bool>> basketContainsSomeTargetProducts =
                d => basket.Items.Any(bi => bi.ProductId == d.TargetProductId);

            var r = await _db.Discounts
                .Where(basketContainsProductInRequiredQuantity)
                .Where(basketContainsSomeTargetProducts)
                .ToListAsync();

            return r;
        }
    }
}
