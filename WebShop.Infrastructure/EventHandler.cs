using System;
using System.Threading.Tasks;
using ApplicationKernel;
using WebShop.Baskets;

namespace WebShop.Infrastructure
{
    public class EventHandler : IEventHandler<BasketSumCalculatedEvent>
    {
        public Task Handle(BasketSumCalculatedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
