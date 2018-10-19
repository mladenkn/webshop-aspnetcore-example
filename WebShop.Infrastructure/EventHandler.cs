using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using WebShop.Baskets;

namespace WebShop.Infrastructure
{
    public abstract class EventHandler : INotificationHandler<BasketSumCalculatedEvent>
    {
        public Task Handle(INotification<BasketSumCalculatedEvent> notification)
        {
            throw new System.NotImplementedException();
        }
    }
}
