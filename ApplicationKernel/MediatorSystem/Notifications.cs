using System.Threading.Tasks;
using MediatR;

namespace ApplicationKernel.MediatorSystem
{
    public interface INotification<out TEvent> : INotification
    {
        TEvent Event { get; }
    }

    public class Notification<TEvent> : INotification<TEvent>
    {
        public Notification(TEvent @event)
        {
            Event = @event;
        }

        public TEvent Event { get; }
    }

    public interface INotificationHandler<in TEvent>
    {
        Task Handle(INotification<TEvent> notification);
    }
}
