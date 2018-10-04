using System.Linq;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Baskets;
using WebShop.Domain.Discounts;
using WebShop.Domain.UseCases.Abstract;
using IRequest = WebShop.Domain.UseCases.Abstract.IRequest;

namespace WebShop.Domain.UseCases
{
    public static class GetDiscounts
    {
        public struct Request : IRequest
        {
            public string UserId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.UserId).NotEmpty();
            }
        }

        public class Handler : Abstract.RequestHandler<Request>
        {
            public Handler(IQueryable<Basket> baskets, DiscountService discountService, IQueryable<Discount> discounts)
            {
                Handle(async (request, cancellationToken) =>
                {
                    var usersBasket = await baskets.FirstOrDefaultAsync(b => b.UserId == request.UserId, cancellationToken);
                    if (usersBasket == null)
                        return Responses.Failure("Users basket not found");
                    var grantedDiscounts = discountService.ApplyDiscounts(usersBasket, discounts);
                    return Responses.Success(grantedDiscounts);
                });
            }
        }
    }
}
