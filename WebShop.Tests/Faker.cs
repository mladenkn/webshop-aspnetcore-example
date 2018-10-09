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
    }
}
