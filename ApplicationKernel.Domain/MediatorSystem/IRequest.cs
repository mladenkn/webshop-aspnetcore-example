using MediatR;

namespace ApplicationKernel.MediatorSystem
{
    public interface IRequest : IRequest<Response>
    {
    }
}
