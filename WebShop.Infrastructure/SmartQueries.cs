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
        private readonly ILowLevelQueries _lowLevelQueries;
        private readonly WebShopDbContext _db;

        public SmartQueries(ILowLevelQueries lowLevelQueries, WebShopDbContext db)
        {
            _lowLevelQueries = lowLevelQueries;
            _db = db;
        }

        public Task<IEnumerable<Discount>> GetPossibleDiscountsFor(Basket basket) =>
            _lowLevelQueries.QueryDiscounts(
                rps => rps.Where(rp => basket.Items.Count(bi => bi.ProductId == rp.ProductId) >= rp.RequiredQuantity),
                mds => mds.Where(md => basket.Items.Any(bi => bi.ProductId == md.TargetProductId))
            );
    }
}
