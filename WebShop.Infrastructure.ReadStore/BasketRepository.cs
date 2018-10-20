﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Infrastructure.ReadStore
{
    public delegate void AddItemToBasket(BasketItem item);

    public class BasketRepository
    {
        private readonly IDataRefreshJobsQueue _jobs;
        private readonly IQueryable<Discount> _discountsTable;
        private readonly IQueryable<Basket> _basketTable;
        private readonly IBasketLowLevelRepository _basketLowLevelRepository;
        private readonly AddDiscountsToBasketItem _addDiscountsToBasketItem;

        public BasketRepository(
            IDataRefreshJobsQueue jobs, 
            IQueryable<Discount> discountsTable, 
            IQueryable<Basket> basketTable,
            IBasketLowLevelRepository basketLowLevelRepository,
            AddDiscountsToBasketItem addDiscountsToBasketItem)
        {
            _jobs = jobs;
            _discountsTable = discountsTable;
            _basketTable = basketTable;
            _basketLowLevelRepository = basketLowLevelRepository;
            _addDiscountsToBasketItem = addDiscountsToBasketItem;
        }

        public async Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            var job = _jobs.Current.OfType<RefreshBasketWithItemJob>().FirstOrDefault(j => j.BasketId == basketId);

            if (job == null)
                return await _basketLowLevelRepository.GetBasketWithDiscountsApplied(basketId);
            else
            {
                await job.Task;
                return await _basketLowLevelRepository.GetBasketWithDiscountsApplied(basketId);
            }
        }

        public void AddItemToBasket(BasketItem item)
        {
            async Task AddActual()
            {
                var productDiscounts = await _discountsTable
                    .Where(d => d.TargetProductId == item.ProductId)
                    .ToArrayAsync();
                item.Basket = await _basketTable.FirstOrDefaultAsync(b => b.Id == item.BasketId);
                _addDiscountsToBasketItem(item, productDiscounts, new List<DiscountGranted>());
                await _basketLowLevelRepository.AddItemToBasket(item);
            }

            var job = new RefreshBasketWithItemJob
            {
                BasketId = item.BasketId,
                BasketItemId = item.Id,
                Task = AddActual()
            };

            _jobs.Add(job);
        }

        internal class RefreshBasketWithItemJob : IDataRefreshJob
        {
            public Task Task { get; set; }
            public int BasketId { get; set; }
            public int BasketItemId { get; set; }
        }
    }
}
