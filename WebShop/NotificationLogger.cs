using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;

namespace WebShop.Domain
{
    public abstract class NotificationLogger : INotificationHandler<INotification>
    {
        public Task Handle(INotification notification, CancellationToken cancellationToken)
        {
            if (ShouldLog(notification))
            {
                var str = Stringify(notification);
                // todo: log actual
            }
            return Task.CompletedTask;
        }

        protected virtual string Stringify(INotification notification) => JsonConvert.SerializeObject(notification);

        protected virtual bool ShouldLog(INotification notification) => true;
    }
}
