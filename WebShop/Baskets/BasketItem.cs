namespace WebShop.Baskets
{
    public class BasketItem
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public User User { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public Product Product { get; set; }
    }
}
