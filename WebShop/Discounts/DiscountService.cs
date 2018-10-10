using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebShop.Abstract;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public class DiscountService : IDiscountService
    {
        private readonly NewTransaction _newTransaction;
        private readonly IMediator _mediator;
        private readonly IBasketService _basketService;

        public DiscountService(NewTransaction newTransaction, IMediator mediator, IBasketService basketService)
        {
            _newTransaction = newTransaction;
            _mediator = mediator;
            _basketService = basketService;
        }

        public async Task Add(Discount discount)
        {
            await _newTransaction().Save(discount).Commit();
            var discountableBasketItems = await _basketService.GetBasketItemsDiscountableWith(discount);
            await discountableBasketItems.Select(i => Discount(i, discount)).WhenAll();
        }

        public Task Discount(BasketItem basketItem, Discount discount)
        {
            basketItem.DiscountId = discount.Id;
            basketItem.Discount = discount;
            return new []
            {
                new DiscountGrantedNotification(basketItem).PublishWith(_mediator),
                _newTransaction().Update(basketItem).Commit()
            }
            .WhenAll();
        }

        public Task<IEnumerable<Discount>> GetDiscountsFor(BasketItem basketItem)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDiscountService
    {
        Task Add(Discount discount);
        Task Discount(BasketItem basketItem, Discount discount);
        Task<IEnumerable<Discount>> GetDiscountsFor(BasketItem basketItem);
    }
}
