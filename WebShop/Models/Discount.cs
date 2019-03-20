using System;
using System.Collections.Generic;

namespace WebShop.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public List<RequiredProduct> RequiredProducts { get; }
        public List<MicroDiscount> MicroDiscounts { get; }


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

        public Discount(int id, List<RequiredProduct> requiredProducts, List<MicroDiscount> microDiscounts)
        {
            Id = id;
            RequiredProducts = requiredProducts;
            MicroDiscounts = microDiscounts;
        }

        public Discount(int id)
        {
            Id = id;
            RequiredProducts = new List<RequiredProduct>();
            MicroDiscounts = new List<MicroDiscount>();
        }

        public void AddRequiredProduct(int productId, int requiredQuantity)
        {
            RequiredProducts.Add(new RequiredProduct {ProductId = productId, RequiredQuantity = requiredQuantity});
        }

        public void AddMicroDiscount(int productId, int quantity, decimal value)
        {
            MicroDiscounts.Add(new MicroDiscount { TargetProductId = productId, MaxNumberOfTargetProductsToBeDiscounted = quantity, Value = value});
        }

        public static Discount Create(int id, Action<Action<int, int>> addRequiredProduct,
            Action<Action<int, int, decimal>> addMicroDiscount)
        {
            var discount = new Discount(id);
            addRequiredProduct(discount.AddRequiredProduct);
            addMicroDiscount(discount.AddMicroDiscount);
            return discount;
        }

        public static Discount Create(int id, Action<Action<int, int>, Action<int, int, decimal>> action)
        {
            var discount = new Discount(id);
            action(discount.AddRequiredProduct, discount.AddMicroDiscount);
            return discount;
        }
    }
}
