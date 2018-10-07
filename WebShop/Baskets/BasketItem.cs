namespace WebShop.Baskets
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BasketId { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public Product Product { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public Basket Basket { get; set; }
        
        /// <summary>  
        ///  Needs initialization from IModelInitializer
        /// </summary> 
        public decimal Price { get; set; }
    }
}
