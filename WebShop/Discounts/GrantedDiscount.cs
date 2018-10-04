namespace WebShop.Domain.Discounts
{
    public struct GrantedDiscount
    {
        public GrantedDiscount(Discount discount, Product product) : this()
        {
            Discount = discount;
            Product = product;
        }

        public Discount Discount { get; }
        public Product Product { get; }

        public decimal PriceWithDiscount
        {
            get
            {
                var without = Product.Price * Discount.Value;
                return Product.Price - without;
            }
        }
    }
}
