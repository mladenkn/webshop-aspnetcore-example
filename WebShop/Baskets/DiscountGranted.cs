namespace WebShop.Baskets
{
    public struct DiscountGranted
    {
        public DiscountGranted(int productId, int discountId) : this()
        {
            ProductId = productId;
            DiscountId = discountId;
        }

        public int ProductId { get; }
        public int DiscountId { get; }
    }
}