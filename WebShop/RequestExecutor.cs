using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebShop.DataAccess;
using WebShop.Logic;
using WebShop.Models;

namespace WebShop
{
    public interface IRequestExecutor
    {
        Task AddItemToBasket(int productId);
    }

    public class RequestExecutor : IRequestExecutor
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IQueries _queries;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;

        public RequestExecutor(ICurrentUserProvider currentUserProvider, IQueries queries, IUnitOfWork unitOfWork, IBasketService basketService)
        {
            _currentUserProvider = currentUserProvider;
            _queries = queries;
            _unitOfWork = unitOfWork;
            _basketService = basketService;
        }

        public async Task AddItemToBasket(int productId)
        {
            var basket = await _queries.GetUsersBasket(_currentUserProvider.GetId());
            var basketItem = new BasketItem {ProductId = productId, BasketId = basket.Id};
            _unitOfWork.Add(basketItem);
            await _basketService.ApplyDiscounts(basket);
            basket.AppliedDiscounts.ForEach(_unitOfWork.Add);
            await _unitOfWork.PersistChanges();
        }

        public Task<Basket> GetUsersBasketWithItemsAndDiscounts(string userId) =>
            _queries.GetUsersBasketWithItemsAndDiscounts(userId);
    }
}
