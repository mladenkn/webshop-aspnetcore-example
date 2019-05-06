using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;

namespace WebShop.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IRequestExecutor _requestExecutor;

        public BasketController(IRequestExecutor requestExecutor)
        {
            _requestExecutor = requestExecutor;
        }

        [HttpPatch]
        public Task AddItem(int productId)
        {
            return _requestExecutor.AddItemToBasket(productId);
        }

        public Task<BasketWithPrice> GetUsersBasketWithItemsAndDiscounts(string userId) =>
            _requestExecutor.GetUsersBasketWithItemsAndDiscounts(userId);
    }
}