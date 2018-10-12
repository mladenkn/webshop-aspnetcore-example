using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Utilities;
using WebShop.Abstract;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public interface IDiscountService
    {
        Task<Discount> Add(Discount discount);
        Task Discount(BasketItem basketItem, Discount discount);
        Task<IEnumerable<Discount>> GetDiscountsFor(BasketItem basketItem);
    }

    public delegate Task ApplyDiscountToBasketItem(BasketItem basketItem, Discount discount);

    public class DiscountService : IDiscountService
    {
        private readonly NewTransaction _newTransaction;
        private readonly IMediator _mediator;
        private readonly IBasketService _basketService;
        private readonly IQueryable<Discount> _discounts;

        public DiscountService(
            NewTransaction newTransaction, 
            IMediator mediator,
            IBasketService basketService,
            IQueryable<Discount> discounts)
        {
            _newTransaction = newTransaction;
            _mediator = mediator;
            _basketService = basketService;
            _discounts = discounts;
        }

        public async Task<Discount> Add(Discount discount)
        {
            await _newTransaction().Save(discount).Commit();
            var discountableBasketItems = await _basketService.GetBasketItemsDiscountableWith(discount);
            await discountableBasketItems.Select(i => Discount(i, discount)).WhenAll();
            return discount;
        }

        public Task Discount(BasketItem basketItem, Discount discount)
        {
            basketItem.IsDiscounted = true;
            var basketDiscount = new BasketItemDiscount {BasketItemId = basketItem.Id, DiscountId = discount.Id};
            return new []
            {
                new DiscountGrantedNotification(basketItem).PublishWith(_mediator),
                _newTransaction().Update(basketItem).Save(basketDiscount).Commit()
            }
            .WhenAll();
        }

        public async Task<IEnumerable<Discount>> GetDiscountsFor(BasketItem basketItem)
        {
            basketItem.Basket.Must().NotBeNull();
            basketItem.Basket.Items.Must().NotBeNull();

            var numberOfProductsInBasket = basketItem.Basket.Items.Count(i => i.ProductId == basketItem.ProductId);

            var discounts = await _discounts
                .Where(d => d.ForProductId == basketItem.ProductId &&
                            numberOfProductsInBasket >= d.RequiredMinimalQuantity)
                .ToListAsync();

            return discounts;
        }
    }
}
