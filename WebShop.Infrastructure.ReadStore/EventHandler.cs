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
        private readonly IDataRefresher _dataRefresher;

        public EventHandler(IDataRefresher dataRefresher)
        {
            _dataRefresher = dataRefresher;
        }

        public Task Process(AddBasketItem.Request request, Response<BasketItem> response)
        {
            if (response.IsSuccess)
            {
                var item = response.Payload;
                _dataRefresher.RefreshBasketWithItemJob(item);
            }
            return Task.CompletedTask;
        }
    }
}
