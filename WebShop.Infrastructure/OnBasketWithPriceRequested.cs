using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel.MediatorSystem;
using MediatR.Pipeline;
using Utilities;
using WebShop.Baskets;
using WebShop.Features;

namespace WebShop.Infrastructure
{
    // instead of using BasketSumCalculatedEvent 
    // this is invoked by mediator after processing GetBasket.Request
    public class OnBasketWithPriceRequested: IRequestPostProcessor<GetBasket.Request, Response<Basket>>
    {
        public Task Process(GetBasket.Request request, Response<Basket> response)
        {
            var basket = response.Payload;

            basket.Items.Must().NotBeNull();
            basket.Items.First().Discounts.Must().NotBeNull();

            // TODO: log

            throw new System.NotImplementedException();
        }
    }
}
