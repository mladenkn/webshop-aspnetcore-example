using System;
using Microsoft.AspNetCore.Mvc;
using WebShop.UseCases;

namespace WebShop.Web.Controllers
{
    [ApiController]
    public class OneController
    {
        [Route("/discounts")]
        public IActionResult Post(AddDiscount.Request request)
        {
            throw new NotImplementedException();
        }

        [Route("/basketitems")]
        public IActionResult Post(AddBasketItem.Request request)
        {
            throw new NotImplementedException();
        }

        [Route("baskets")]
        public IActionResult Get(GetBasket.Request request)
        {
            throw new NotImplementedException();
        }
    }
}
