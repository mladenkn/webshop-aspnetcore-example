using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.DataAccess
{
    // Dummy implementation
    // I just did IBasketWithPriceCache to show it is a good idea, but i think it's out of scope of the assignment
    public class BasketWithPriceCache : IBasketWithPriceCache
    {
        public Task<BasketWithPrice> Get(int basketId)
        {
            throw new NotImplementedException();
        }

        public Task Set(BasketWithPrice b)
        {
            return Task.CompletedTask;
        }

        public void Invalidate(int basketId)
        {
        }

        public bool IsValid(int basketId) => false;
    }
}
