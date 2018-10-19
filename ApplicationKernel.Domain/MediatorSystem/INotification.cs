using MediatR;

namespace ApplicationKernel.Domain.MediatorSystem
{
    public interface INotification<out TEvent> : INotification
    {
        TEvent Event { get; }
    }
}
