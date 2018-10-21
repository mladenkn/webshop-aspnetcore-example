using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Tests.GetBasketWithDiscountsAppliedTest
{
    internal class DataContainer
    {
        internal Basket Basket { get; } = new Basket { Id = 1 };

        internal Product Butter { get; } = new Product
        {
            Id = 1,
            RegularPrice = 0.8m
        };

        internal Product Milk { get; } = new Product
        {
            Id = 2,
            RegularPrice = 1.15m
        };

        internal Product Bread { get; } = new Product
        {
            Id = 3,
            RegularPrice = 1.0m
        };

        internal Discount BreadDiscount { get; }
        internal Discount MilkDiscount { get; }

        internal DataContainer()
        {
            MilkDiscount = new Discount
            {
                TargetProductId = Milk.Id,
                RequiredProductId = Milk.Id,
                RequiredProductQuantity = 3,
                TargetProductQuantity = 1,
                Value = 1
            };
            BreadDiscount = new Discount
            {
                TargetProductId = Bread.Id,
                RequiredProductId = Butter.Id,
                RequiredProductQuantity = 2,
                TargetProductQuantity = 1,
                Value = 0.5m
            };
        }

        internal BasketItem BasketItem(Product product)
        {
            return new BasketItem
            {
                Basket = Basket,
                BasketId = Basket.Id,
                Product = product,
                ProductId = product.Id
            };
        }
    }
}