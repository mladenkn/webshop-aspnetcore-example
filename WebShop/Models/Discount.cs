using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public IReadOnlyCollection<RequiredProduct> RequiredProducts { get; }
        public IReadOnlyCollection<MicroDiscount> MicroDiscounts { get; }


        public class RequiredProduct
        {
            public int ProductId { get; set; }
            public int RequiredQuantity { get; set; }
        }

        public class MicroDiscount
        {
            public int TargetProductId { get; set; }
            public int MaxNumberOfTargetProductsToBeDiscounted { get; set; }
            public decimal Value { get; set; }
        }

        public Discount(int id, IReadOnlyCollection<RequiredProduct> requiredProducts,
            IReadOnlyCollection<MicroDiscount> microDiscounts)
        {
            Id = id;
            RequiredProducts = requiredProducts;
            MicroDiscounts = microDiscounts;
        }

        public static DiscountBuilder New() => new DiscountBuilder();
    }

    public class DiscountBuilder
    {
        private int _id;
        private readonly List<Discount.RequiredProduct> _requiredProducts = new List<Discount.RequiredProduct>();
        private readonly List<Discount.MicroDiscount> _microDiscounts = new List<Discount.MicroDiscount>();

        public DiscountBuilder Require(int productId, int requiredQuantity)
        {
            _requiredProducts.Add(new Discount.RequiredProduct { ProductId = productId, RequiredQuantity = requiredQuantity });
            return this;
        }

        public DiscountBuilder DiscountFor(int productId, int quantity, decimal value)
        {
            _microDiscounts.Add(new Discount.MicroDiscount { TargetProductId = productId, MaxNumberOfTargetProductsToBeDiscounted = quantity, Value = value });
            return this;
        }

        public DiscountBuilder Id(int id)
        {
            _id = id;
            return this;
        }

        public Discount Build() => new Discount(_id, _requiredProducts, _microDiscounts);
    }
}
