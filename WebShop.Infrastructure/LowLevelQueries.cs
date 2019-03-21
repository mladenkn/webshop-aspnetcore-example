using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    public interface ILowLevelQueries
    {
        Task<IEnumerable<Discount>> QueryDiscounts(Query<Discount.RequiredProduct> rpQuery, Query<Discount.MicroDiscount> mdQuery);
    }

    public class LowLevelQueries : ILowLevelQueries
    {
        private readonly WebShopDbContext _db;

        public LowLevelQueries(WebShopDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Discount>>
            QueryDiscounts(Query<Discount.RequiredProduct> rpQuery, Query<Discount.MicroDiscount> mdQuery)
        {
            var requiredProductsOfDiscount = await rpQuery(_db.RequiredProductsOfDiscounts).ToListAsync();
            var microDiscounts = await mdQuery(_db.MicroDiscounts).ToListAsync();

            var bothModelsByDiscount =
                new Dictionary<Guid, (List<Discount.RequiredProduct> rps, List<Discount.MicroDiscount> mds)>();

            var allDiscountIds = Enumerable
                .Concat(requiredProductsOfDiscount.Select(m => m.DiscountId), 
                        microDiscounts.Select(m => m.DiscountId))
                .Distinct();

            foreach (var discountId in allDiscountIds)
                bothModelsByDiscount[discountId] =
                    (new List<Discount.RequiredProduct>(), new List<Discount.MicroDiscount>());

            foreach (var rp in requiredProductsOfDiscount)
                bothModelsByDiscount[rp.DiscountId].rps.Add(rp);

            foreach (var md in microDiscounts)
                bothModelsByDiscount[md.DiscountId].mds.Add(md);

            var discounts = bothModelsByDiscount.Select(m => Discount.FromDbModels(m.Value.rps, m.Value.mds));

            return discounts;
        }
    }
}
