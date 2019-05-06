using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebShop.DataAccess;
using WebShop.Models;
using WebShop.Services;

namespace WebShop
{
    public interface IRequestExecutor
    {
        Task AddItemToBasket(int productId);
        Task<BasketWithPrice> GetUsersBasketWithItemsAndDiscounts(string userId);
    }

    public class RequestExecutor : IRequestExecutor
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IQueries _queries;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketWithPriceCache _basketCache;
        private readonly IBasketService _basketService;

        public RequestExecutor(
            ICurrentUserProvider currentUserProvider, 
            IQueries queries,
            IUnitOfWork unitOfWork,
            IBasketWithPriceCache basketCache,
            IBasketService basketService)
        {
            _currentUserProvider = currentUserProvider;
            _queries = queries;
            _unitOfWork = unitOfWork;
            _basketCache = basketCache;
            _basketService = basketService;
        }

        public async Task AddItemToBasket(int productId)
        {
            var basket = await _queries.GetUsersBasket(_currentUserProvider.GetId());
            var basketItem = new BasketItem {ProductId = productId, BasketId = basket.Id};
            _unitOfWork.Add(basketItem);
            _basketCache.Invalidate(basket.Id);
            await _unitOfWork.PersistChanges();
        }

        public async Task<BasketWithPrice> GetUsersBasketWithItemsAndDiscounts(string userId)
        {
            var basketWithoutPrice = await _queries.GetUsersBasketWithItemsAndDiscounts(userId);
            if (_basketCache.IsValid(basketWithoutPrice.Id))
                return await _basketCache.Get(basketWithoutPrice.Id);
            else
            {
                var basketWithPrice = await _basketService.CalculatePrice(basketWithoutPrice);
                await _basketCache.Set(basketWithPrice);
                return basketWithPrice;
            }
        }
    }
}
