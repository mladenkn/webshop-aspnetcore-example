using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;

namespace WebShop.UseCases
{
    public class GetBasket
    {
        public struct Request : IRequest
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {

            }
        }

        public class Handler : IRequestHandler<Request>
        {
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
