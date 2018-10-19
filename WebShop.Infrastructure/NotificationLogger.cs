using System;
using System.Threading;
using System.Threading.Tasks;
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
}
