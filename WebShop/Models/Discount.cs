using System.Collections.Generic;

namespace WebShop.Models
{
    public class Discount
    {
        public class RequiredProduct
        {
            public int ProductId { get; set; }
            public int RequiredQuantity { get; set; }
        }

        public class MicroDiscount
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Value { get; set; }
        }

        public int Id { get; set; }
        public List<RequiredProduct> RequiredProducts { get; set; }
        public List<MicroDiscount> MicroDiscounts { get; set; }

        public void AddRequiredProduct(int productId, int requiredQuantity)
        {
            RequiredProducts.Add(new RequiredProduct {ProductId = productId, RequiredQuantity = requiredQuantity});
        }

        public void AddMicroDiscount(int productId, int quantity, decimal value)
        {
            MicroDiscounts.Add(new MicroDiscount { ProductId = productId, Quantity = quantity, Value = value});
        }
    }
}
