using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bogus;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Tests
{
    public static class Faker
    {
        public static Faker<BasketItem> RuleForProduct(this Faker<BasketItem> faker, Product product)
        {
            return faker
                .RuleFor(i => i.Product, product)
                .RuleFor(i => i.ProductId, product.Id);
        }

        public static Faker<BasketItem> RuleForBasket(this Faker<BasketItem> faker, Basket basket)
        {
            return faker
                .RuleFor(i => i.Basket, basket)
                .RuleFor(i => i.BasketId, basket.Id);
        }

        public static Faker<Discount> RuleForProduct(this Faker<Discount> faker, Product product)
        {
            return faker
                .RuleFor(i => i.Product, product)
                .RuleFor(i => i.ProductId, product.Id);
        }

        public static Faker<GrantedDiscount> RuleForDiscount(this Faker<GrantedDiscount> faker, Discount discount)
        {
            return faker
                .RuleFor(i => i.Discount, discount)
                .RuleFor(i => i.DiscountId, discount.Id);
        }

        public static Faker<T> EmptyCollectionRuleFor<T, TListItem>(
            this Faker<T> faker, Expression<Func<T, IReadOnlyCollection<TListItem>>> list)
            where T : class
        {
            return faker.RuleFor(list, new TListItem[0]);
        }
    }
}
