using MediatR;

namespace ApplicationKernel.Domain.MediatorSystem
{
    public interface IRequestHandler<in TRequest>
        : IRequestHandler<TRequest, Response>
        where TRequest : IRequest<Response>
    {
    }
}
