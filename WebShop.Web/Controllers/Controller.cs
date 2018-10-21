﻿using System.Threading.Tasks;
using ApplicationKernel.Infrastructure.RestApi;
using Microsoft.AspNetCore.Mvc;
using WebShop.Features;

namespace WebShop.Infrastructure.RestApi.Controllers
{
    [ApiController]
    public class Controller : ApiController
    {
        public Controller(HandleApiRequest handle) : base(handle)
        {
        }

        [HttpPost]
        [Route("/discounts")]
        public Task<IActionResult> Post(AddDiscount.Request request) => Handle(request);

        [HttpPost]
        [Route("/basketitems")]
        public Task<IActionResult> Post(AddBasketItem.Request request) => Handle(request);

        [HttpGet]
        [Route("baskets")]
        public Task<IActionResult> Get(int basketId) => Handle(new GetBasket.Request{Id = basketId});
    }
}