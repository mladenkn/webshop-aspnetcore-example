using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using MediatR.Pipeline;
using WebShop.BasketItems;
using WebShop.Features;

namespace WebShop.Infrastructure.ReadStore
{
    public class EventHandler : IRequestPostProcessor<AddBasketItem.Request, Response<BasketItem>>
    {
        private readonly AddItemToBasket _addItemToBasket;

        public EventHandler(AddItemToBasket addItemToBasket)
        {
            _addItemToBasket = addItemToBasket;
        }

        public Task Process(AddBasketItem.Request request, Response<BasketItem> response)
        {
            if (response.IsSuccess)
                _addItemToBasket(response.Payload);
            return Task.CompletedTask;
        }
    }
}
