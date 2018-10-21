using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Tests.GetBasketWithDiscountsApplied
{
    internal class DataContainer
    {
        public Basket Basket { get; } = new Basket { Id = 1 };

        public Product Butter { get; } = new Product
        {
            Id = 1,
            RegularPrice = 0.8m
        };

        public Product Milk { get; } = new Product
        {
            Id = 2,
            RegularPrice = 1.15m
        };

        public Product Bread { get; } = new Product
        {
            Id = 3,
            RegularPrice = 1.0m
        };

        public Discount BreadDiscount { get; }
        public Discount MilkDiscount { get; }

        public DataContainer()
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

        public BasketItem BasketItem(Product product)
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