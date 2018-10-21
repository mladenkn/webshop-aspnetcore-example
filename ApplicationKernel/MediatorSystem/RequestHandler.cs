using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ApplicationKernel.MediatorSystem
{
    public interface IRequestHandler<in TRequest>
        : IRequestHandler<TRequest, Response>
        where TRequest : IRequest<Response>
    {
    }

    public class RequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest<Response>
    {
        private Func<TRequest, CancellationToken, Task<Response>> _handler;

        public Task<Response> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return _handler(request, cancellationToken);
        }

        protected void HandleWith(Func<TRequest, CancellationToken, Task<Response>> handler)
        {
            _handler = handler;
        }
    }
}
