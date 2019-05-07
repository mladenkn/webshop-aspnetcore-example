using System;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.DataAccess
{
    public interface IQueries
    {
        Task<Basket> GetUsersBasket(string userId, Action<IncludesBuilder<Basket>> includeProps);
        Task<BasketItem> GetBasketItem(int id);
    }
}
