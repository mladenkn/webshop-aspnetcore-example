using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ApplicationKernel
{
    public class RequestHandler<TRequest> : IRequestHandler<TRequest, Response>
        where TRequest : IRequest<Response>
    {
        private Func<TRequest, CancellationToken, Task<Response>> _func;

        protected void Handle(Func<TRequest, CancellationToken, Task<Response>> func)
        {
            _func = func;
        }

        public Task<Response> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return _func(request, cancellationToken);
        }
    }
}
