using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using WebShop.Baskets;

namespace WebShop.Features
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

        public class Handler : RequestHandler<Request>
        {
            public Handler(GetBasketWithDiscountsApplied get)
            {
                HandleWith(async (request, token) =>
                {
                    var basket = await get(request.Id);
                    return basket == null ?
                        Responses.Failure("Basket not found") :
                        Responses.Success(basket);
                });
            }
        }
    }
}
