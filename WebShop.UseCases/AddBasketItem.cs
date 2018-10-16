using ApplicationKernel.Domain.MediatorSystem;
using AutoMapper;
using FluentValidation;
using WebShop.Abstract;
using WebShop.Baskets;

namespace WebShop.Features
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
            public Handler(NewTransaction newTransaction, IMapper mapper)
            {
                HandleWith(async (request, cancellationToken) =>
                {
                    var basketItem = mapper.Map<BasketItem>(request);
                    await newTransaction().Save(basketItem).Commit();
                    return Responses.Success(basketItem);
                });
            }
        }
    }
}
