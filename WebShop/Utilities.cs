using System.Threading.Tasks;
using MediatR;

namespace WebShop
{
    public static class Utilities
    {
        public static Task PublishWith<T>(this T notification, IMediator mediator)
            where T : INotification
        {
            return mediator.Publish(notification);
        }
    }
}
