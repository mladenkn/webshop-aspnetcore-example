using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using MediatR.Pipeline;
using WebShop.BasketItems;
using WebShop.Features;
using WebShop.Infrastructure.ReadStore.Refreshing;

namespace WebShop.Infrastructure.ReadStore
{
    public class EventHandler : IRequestPostProcessor<AddBasketItem.Request, Response<BasketItem>>
    {
        private readonly RefreshBasketWithItem _refreshBasketWithItem;

        public EventHandler(RefreshBasketWithItem refreshBasketWithItem)
        {
            _refreshBasketWithItem = refreshBasketWithItem;
        }

        public Task Process(AddBasketItem.Request request, Response<BasketItem> response)
        {
            if (!response.IsSuccess)
                return Task.CompletedTask;
            var item = response.Payload;
            _refreshBasketWithItem(item);
            return Task.CompletedTask;
        }
    }
}
