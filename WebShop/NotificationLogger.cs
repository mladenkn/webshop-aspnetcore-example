using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;

namespace WebShop
{
    public abstract class NotificationLogger : INotificationHandler<INotification>
    {
        public Task Handle(INotification notification, CancellationToken cancellationToken)
        {
            var str = JsonConvert.SerializeObject(notification);
            Console.WriteLine(str);
            return Task.CompletedTask;
        }
    }
}
