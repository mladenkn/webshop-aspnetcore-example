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

        public class Handler : RequestHandler<Request>
        {
            public Handler(IBasketService basketService)
            {
                HandleWith(async (request, token) =>
                {
                    var basket = await basketService.GetBasket(request.Id);
                    return basket == null ? 
                        Responses.Failure("Basket not found") :
                        Responses.Success(basket);
                });
            }
        }
    }
}
