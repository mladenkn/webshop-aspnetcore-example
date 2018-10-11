using System;
using System.Threading;
using System.Threading.Tasks;
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

        public class Handler : IRequestHandler<Request>
        {
            private readonly IMapper _mapper;
            private readonly IDiscountService _service;

            public Handler(IMapper mapper, IDiscountService service)
            {
                _mapper = mapper;
                _service = service;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var discount = _mapper.Map<Discount>(request);
                discount = await _service.Add(discount);
                return Responses.Success(discount);
            }
        }
    }
}
