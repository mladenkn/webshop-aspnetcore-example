using System.Threading.Tasks;
using MediatR;

namespace ApplicationKernel.Domain.MediatorSystem
{
    public static class MediatorExtensions
    {
        public static Task Publish<TEvent>(this IMediator mediator, TEvent @event)
        {
            var notification = new Notification<TEvent>(@event);
            return mediator.Publish(notification);
        }
    }
}
