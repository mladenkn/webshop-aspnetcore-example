using System;
using System.Collections.Generic;
using System.Linq;

namespace WebShop.Models
{
    public class Discount
    {
        public Guid Id { get; }
        public IReadOnlyCollection<RequiredProduct> RequiredProducts { get; }
        public IReadOnlyCollection<MicroDiscount> MicroDiscounts { get; }

        public class RequiredProduct
        {
            public Guid DiscountId { get; set; }
            public int ProductId { get; set; }
            public int RequiredQuantity { get; set; }
        }

        public class MicroDiscount
        {
            public Guid DiscountId { get; set; }
            public int TargetProductId { get; set; }
            public int MaxNumberOfTargetProductsToBeDiscounted { get; set; }
            public decimal Value { get; set; }
        }
        
        public Discount(Guid id, IReadOnlyCollection<RequiredProduct> requiredProducts,
            IReadOnlyCollection<MicroDiscount> microDiscounts)
        {
            Id = id;
            RequiredProducts = requiredProducts;
            MicroDiscounts = microDiscounts;
        }

        public static DiscountBuilder New() => new DiscountBuilder();

        public static Discount FromDbModels(IReadOnlyCollection<RequiredProduct> requiredProducts,
            IReadOnlyCollection<MicroDiscount> microDiscounts)
        {
            var id = requiredProducts.First().DiscountId;
            return new Discount(id, requiredProducts, microDiscounts);
        }
    }

    public class DiscountBuilder
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly List<Discount.RequiredProduct> _requiredProducts = new List<Discount.RequiredProduct>();
        private readonly List<Discount.MicroDiscount> _microDiscounts = new List<Discount.MicroDiscount>();

        public DiscountBuilder Require(int productId, int requiredQuantity)
        {
            _requiredProducts.Add(new Discount.RequiredProduct
            {
                DiscountId = _id, ProductId = productId, RequiredQuantity = requiredQuantity
            });
            return this;
        }

        public DiscountBuilder DiscountFor(int productId, int quantity, decimal value)
        {
            _microDiscounts.Add(new Discount.MicroDiscount
            {
                DiscountId = _id,
                TargetProductId = productId,
                MaxNumberOfTargetProductsToBeDiscounted = quantity,
                Value = value
            });
            return this;
        }

        public Discount Build() => new Discount(_id, _requiredProducts, _microDiscounts);
    }
}
