using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using AutoMapper;
using FluentValidation;
using WebShop.Baskets;
using AutoMapperExtension = ApplicationKernel.Domain.AutoMapperExtension;

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
        
        public class Handler : IRequestHandler<Request>
        {
            private readonly IBasketService _basketService;
            private readonly IMapper _mapper;

            public Handler(IBasketService basketService, IMapper mapper)
            {
                _basketService = basketService;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var basketItem = _mapper.Map<BasketItem>(request);
                basketItem = await _basketService.AddBasketItem(basketItem);
                return Responses.Success(basketItem);
            }
        }
    }
}
