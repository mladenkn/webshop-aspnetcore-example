using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using WebShop.Baskets;
using WebShop.Discounts;
using WebShop.Tests.Abstract;
using Xunit;

namespace WebShop.Tests
{
    public class BasketItemInitializerShould : FakerContainerAware
    {
        private Basket _basket;
        private Product _product;

        private void Init(int numberOfGrantedDiscounts, int basketItemId = 1)
        {
            FakerOf<Product>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.RegularPrice, 50)
                ;

            FakerOf<Basket>()
                .RuleFor(b => b.Id, 1)
                .RuleFor(b => b.GrantedDiscounts, f => FakerOf<GrantedDiscount>().Generate(numberOfGrantedDiscounts))
                ;

            FakerOf<BasketItem>()
                .RuleFor(i => i.Id, basketItemId)
                .RuleFor(i => i.Basket, f => Basket)
                .RuleFor(i => i.BasketId, f => Basket.Id)
                .RuleFor(i => i.ProductId, f => Product.Id)
                .RuleFor(i => i.Product, f => Product)
                ;
        }

        private Basket Basket => _basket ?? (_basket = FakerOf<Basket>().Generate());
        private Product Product => _product ?? (_product = FakerOf<Product>().Generate());

        [Fact]
        public async Task Set_price_equal_to_regular_price()
        {
            Init(numberOfGrantedDiscounts: 0);

            var item = FakerOf<BasketItem>().Generate();

            var sut = new BasketItemService();
            await sut.Initialize(item);
            item.Price.Should().Be(Product.RegularPrice);
        }

        [Fact]
        public async Task Set_discounted_price()
        {
            var basketItemId = 1;
            Init(numberOfGrantedDiscounts: 1, basketItemId: basketItemId);

            var discount = FakerOf<Discount>()
                .RuleFor(p => p.ProductId, Product.Id)
                .RuleFor(p => p.MaxNumberOfItemsToApplyTo, 1)
                .RuleFor(p => p.RequiredMinimalQuantity, 1)
                .RuleFor(p => p.Value, (decimal) 0.1)
                .Generate();

            FakerOf<GrantedDiscount>()
                .RuleFor(gd => gd.DiscountId, discount.Id)
                .RuleFor(gd => gd.Discount, discount)
                .RuleFor(gd => gd.ItemId, basketItemId)
                ;

            var item = FakerOf<BasketItem>().Generate();

            var sut = new BasketItemService();
            await sut.Initialize(item);
            item.Price.Should().Be(45);
        }
    }
}
