using System.Threading.Tasks;
using ApplicationKernel.MediatorSystem;
using MediatR.Pipeline;
using WebShop.BasketItems;
using WebShop.Features;

namespace WebShop.Infrastructure.PersistentCache
{
    public class EventHandler : IRequestPostProcessor<AddBasketItem.Request, Response<BasketItem>>
    {
        private readonly IBasketsWithDiscountsCache _cache;

        public EventHandler(IBasketsWithDiscountsCache cache)
        {
            _cache = cache;
        }

        public Task Process(AddBasketItem.Request request, Response<BasketItem> response)
        {
            if (response.IsSuccess)
                _cache.AddItem(response.Payload);
            return Task.CompletedTask;
        }
    }
}
