namespace WebShop.Domain.Baskets
{
    public class BasketItem
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
            
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
