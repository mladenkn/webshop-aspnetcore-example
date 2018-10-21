using System;
using System.Threading.Tasks;
using ApplicationKernel;
using WebShop.Baskets;

namespace WebShop.Infrastructure
{
    public class Logger : IEventHandler<BasketSumCalculatedEvent>
    {
        public Task Handle(BasketSumCalculatedEvent @event)
        {
            // TODO: log
            throw new NotImplementedException();
        }
    }
}
