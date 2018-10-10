using System;
using ApplicationKernel.Infrastructure.RestApi;
using Microsoft.AspNetCore.Mvc;
using WebShop.UseCases;

namespace WebShop.Web.Controllers
{
    [ApiController]
    public class OneController
    {
        private readonly HandleApiRequest _handle;

        public OneController(HandleApiRequest handle)
        {
            _handle = handle;
        }

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
