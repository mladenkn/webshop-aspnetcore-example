using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using MediatR;

namespace WebShop.Infrastructure
{
    public abstract class NotificationLogger : INotificationHandler<INotification>
    {
        public Task Handle(INotification notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class NotificationLogger<TEvent> : INotificationHandler<INotification<TEvent>>
    {
        public Task Handle(INotification<TEvent> notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
