using WebShop.Discounts;

namespace WebShop.Baskets
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BasketId { get; set; }
        public int? DiscountId { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public Product Product { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public Basket Basket { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public Discount Discount { get; set; }

        /// <summary>  
        ///  Needs initialization from IModelInitializer
        /// </summary> 
        public decimal Price { get; set; }

        public bool IsDiscounted => DiscountId != null;
    }
}
