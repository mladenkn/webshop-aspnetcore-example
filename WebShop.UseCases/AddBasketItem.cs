using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;

namespace WebShop.UseCases
{
    public class AddBasketItem
    {
        public struct Request : IRequest
        {
            public int ProductId { get; set; }
            public int BasketId { get; set; }
        }

        public class Validator : AbstractValidator<GetBasket.Request>
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
