using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.DataAccess;
using WebShop.Logic;
using WebShop.Models;

namespace WebShop
{
    public interface IRequestExecutor
    {
        Task<Basket> AddItemToBasket(int productId);
    }

    public class RequestExecutor : IRequestExecutor
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        //private readonly IBasketService _basketService;
        private readonly IQueries _queries;

        public RequestExecutor(ICurrentUserProvider currentUserProvider, IQueries queries)
        {
            _currentUserProvider = currentUserProvider;
            //_basketService = basketService;
            _queries = queries;
        }

        public async Task<Basket> AddItemToBasket(int productId)
        {
            var userId = _currentUserProvider.GetId();
            var basket = await _queries.GetUsersBasketWithItems(userId);
            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                // todo: persist basket
            }
            //await _basketService.AddItem(basket, productId);
            return basket;
        }
    }
}
