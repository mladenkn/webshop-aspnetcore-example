using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
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
                
            }
        }

        public class Handler : IRequestHandler<Request>
        {
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
