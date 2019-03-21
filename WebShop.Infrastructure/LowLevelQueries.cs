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
        Task<IEnumerable<Discount>> QueryDiscounts(Query<RequiredProductOfDiscount> rpQuery, Query<MicroDiscount> mdQuery);
    }

    public class LowLevelQueries : ILowLevelQueries
    {
        private readonly WebShopDbContext _db;
        private readonly ICustomMapper _mapper;

        public LowLevelQueries(WebShopDbContext db, ICustomMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Discount>>
            QueryDiscounts(Query<RequiredProductOfDiscount> rpQuery, Query<MicroDiscount> mdQuery)
        {
            var requiredProductsOfDiscount = await rpQuery(_db.RequiredProductsOfDiscounts).ToListAsync();
            var microDiscounts = await mdQuery(_db.MicroDiscounts).ToListAsync();

            var bothModelsByDiscount =
                new Dictionary<Guid, (List<RequiredProductOfDiscount> rps, List<MicroDiscount> mds)>();

            void AddRp(Guid discountId, RequiredProductOfDiscount rp)
            {
                if (!bothModelsByDiscount.ContainsKey(discountId))
                {
                    bothModelsByDiscount[discountId] =
                        (new List<RequiredProductOfDiscount>(), new List<MicroDiscount>());
                }
                bothModelsByDiscount[discountId].rps.Add(rp);
            }

            void AddMd(Guid discountId, MicroDiscount md)
            {
                if (!bothModelsByDiscount.ContainsKey(discountId))
                {
                    bothModelsByDiscount[discountId] =
                        (new List<RequiredProductOfDiscount>(), new List<MicroDiscount>());
                }
                bothModelsByDiscount[discountId].mds.Add(md);
            }

            foreach (var rp in requiredProductsOfDiscount)
                AddRp(rp.DiscountId, rp);

            foreach (var md in microDiscounts)
                AddMd(md.DiscountId, md);

            var discounts = bothModelsByDiscount.Select(m => _mapper.ToDomainModel(m.Value.rps, m.Value.mds));

            return discounts;
        }
    }
}
