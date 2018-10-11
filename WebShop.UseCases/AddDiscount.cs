using ApplicationKernel.Domain.MediatorSystem;
using AutoMapper;
using FluentValidation;
using WebShop.Discounts;
using IRequest = ApplicationKernel.Domain.MediatorSystem.IRequest;

namespace WebShop.UseCases
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
            public Handler(IMapper mapper, IDiscountService service)
            {
                HandleWith(async (request, token) =>
                {
                    var discount = mapper.Map<Discount>(request);
                    discount = await service.Add(discount);
                    return Responses.Success(discount);
                });
            }
        }
    }
}
