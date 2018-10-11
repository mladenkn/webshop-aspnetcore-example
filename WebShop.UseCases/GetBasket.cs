using System.Threading;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using WebShop.Baskets;

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
                RuleFor(r => r.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Request>
        {
            private readonly IBasketService _basketService;

            public Handler(IBasketService basketService)
            {
                _basketService = basketService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var basket = await _basketService.GetBasket(request.Id);
                return Responses.Success(basket);
            }
        }
    }
}
