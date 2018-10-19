using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using MediatR.Pipeline;
using WebShop.Baskets;
using WebShop.Features;

namespace WebShop.Infrastructure
{
    // insted of using BasketSumCalculatedEvent 
    // this is invoked by mediator after processing GetBasket.Request
    public class OnBasketWithPriceRequested: IRequestPostProcessor<GetBasket.Request, Response<Basket>>
    {
        public Task Process(GetBasket.Request request, Response<Basket> response)
        {
            // TODO: log
            throw new System.NotImplementedException();
        }
    }
}
