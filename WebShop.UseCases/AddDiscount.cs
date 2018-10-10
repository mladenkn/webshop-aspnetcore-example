using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace WebShop.UseCases
{
    public class AddDiscount
    {
        public struct Request : IRequest
        {

        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                
            }
        }

        public class Handler : IRequestHandler<Request>
        {
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
