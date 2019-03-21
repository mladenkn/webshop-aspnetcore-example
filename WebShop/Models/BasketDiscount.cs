using System;
using System.Collections.Generic;
using System.Linq;

namespace WebShop.Models
{
    public class BasketDiscount
    {
        public Guid Id { get; set; }
        public IReadOnlyCollection<RequiredProduct> RequiredProducts { get; set; }
        public IReadOnlyCollection<BasketItemDiscount> BasketItemDiscounts { get; set; }

        public class RequiredProduct
        {
            public Guid BasketDiscountId { get; set; }
            public int ProductId { get; set; }
            public int RequiredQuantity { get; set; }
        }

        public class BasketItemDiscount
        {
            public int Id { get; set; }
            public Guid BasketDiscountId { get; set; }
            public int TargetProductId { get; set; }
            public int MaxNumberOfTargetProductsToBeDiscounted { get; set; }
            public decimal Value { get; set; }
        }

        public static BasketDiscountBuilder New() => new BasketDiscountBuilder();
    }

    public class BasketDiscountBuilder
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly List<BasketDiscount.RequiredProduct> _requiredProducts = new List<BasketDiscount.RequiredProduct>();
        private readonly List<BasketDiscount.BasketItemDiscount> _microDiscounts = new List<BasketDiscount.BasketItemDiscount>();

        public BasketDiscountBuilder Require(int productId, int requiredQuantity)
        {
            _requiredProducts.Add(new BasketDiscount.RequiredProduct
            {
                BasketDiscountId = _id, ProductId = productId, RequiredQuantity = requiredQuantity
            });
            return this;
        }

        public BasketDiscountBuilder DiscountFor(int productId, int quantity, decimal value)
        {
            _microDiscounts.Add(new BasketDiscount.BasketItemDiscount
            {
                BasketDiscountId = _id,
                TargetProductId = productId,
                MaxNumberOfTargetProductsToBeDiscounted = quantity,
                Value = value
            });
            return this;
        }

        public BasketDiscount Build() => new BasketDiscount
        {
            Id = _id, RequiredProducts = _requiredProducts, BasketItemDiscounts = _microDiscounts
        };
    }
}
