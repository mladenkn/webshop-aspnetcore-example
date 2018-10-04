using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.UseCases;

namespace WebShop.Rest.Controllers
{
    [ApiController]
    public class BasketController : BaseController
    {
        [HttpGet]
        public Task<IActionResult> Discounts(string userId) => Handle(new GetDiscounts.Request { UserId = userId });
    }
}
