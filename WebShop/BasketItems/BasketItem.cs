using System.Collections.Generic;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.BasketItems
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BasketId { get; set; }
        public bool IsDiscounted { get; set; }
        
        public Product Product { get; set; }
        public Basket Basket { get; set; }

        /// <summary>  
        ///  Not persisted
        /// </summary> 
        public IReadOnlyCollection<Discount> Discounts { get; set; }

        /// <summary>  
        ///  Not persisted
        /// </summary> 
        public decimal Price { get; set; }
    }
}
