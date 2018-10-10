using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

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

        public class Handler : IRequestHandler<GetBasket.Request>
        {
            public Task<Unit> Handle(GetBasket.Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
