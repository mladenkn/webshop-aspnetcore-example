using System.Threading.Tasks;

namespace WebShop.Abstract
{
    public interface IEventDispatcher
    {
        Task Dispatch<TEvent>(TEvent @event);
    }

    public interface IEventHandler<in TEvent>
    {
        Task Handle(TEvent @event);
    }
}
