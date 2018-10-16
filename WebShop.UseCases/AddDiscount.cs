using ApplicationKernel.Domain.MediatorSystem;
using AutoMapper;
using FluentValidation;
using WebShop.Abstract;
using WebShop.Discounts;

namespace WebShop.Features
{
    public class AddDiscount
    {
        public struct Request : IRequest
        {
            public int ForProductId { get; set; }
            public int RequiredMinimalQuantity { get; set; }
            public decimal Value { get; set; }
            public int MaxNumberOfItemsToApplyTo { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.ForProductId).GreaterThan(0);
                RuleFor(r => r.RequiredMinimalQuantity).GreaterThan(0);
                RuleFor(r => r.Value).GreaterThan(0);
                RuleFor(r => r.MaxNumberOfItemsToApplyTo).GreaterThan(0);
            }
        }

        public class Handler : RequestHandler<Request>
        {
            public Handler(IMapper mapper, NewTransaction newTransaction)
            {
                HandleWith(async (request, token) =>
                {
                    var discount = mapper.Map<Discount>(request);
                    await newTransaction().Save(discount).Commit();
                    return Responses.Success(discount);
                });
            }
        }
    }
}
