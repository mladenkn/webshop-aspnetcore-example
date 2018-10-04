using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.UseCases;

namespace WebShop.Rest.Controllers
{
    public class BasketController
    {
        private readonly GetDiscounts.Handler _handler;

        public BasketController(GetDiscounts.Handler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public async Task<IActionResult> Discounts(GetDiscounts.Request request)
        {
            var validator = new GetDiscounts.Validator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(new
                {
                    validationResult.Errors
                });
            }
            else
            {
                var response = await _handler.Handle(request, default);
                return null;
            }
        }
    }
}
