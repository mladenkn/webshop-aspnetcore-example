using ApplicationKernel.Domain.MediatorSystem;
using AutoMapper;
using FluentValidation;
using WebShop.Baskets;

namespace WebShop.UseCases
{
    public class AddBasketItem
    {
        public struct Request : IRequest
        {
            public int ProductId { get; set; }
            public int BasketId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.ProductId).GreaterThan(0);
                RuleFor(r => r.BasketId).GreaterThan(0);
            }
        }
        
        public class Handler : RequestHandler<Request>
        {
            public Handler(IBasketService basketService, IMapper mapper)
            {
                HandleWith(async (request, cancellationToken) =>
                {
                    var basketItem = mapper.Map<BasketItem>(request);
                    basketItem = await basketService.AddBasketItem(basketItem);
                    return Responses.Success(basketItem);
                });
            }
        }
    }
}
