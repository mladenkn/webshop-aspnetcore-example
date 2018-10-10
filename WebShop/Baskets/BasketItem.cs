using System.Collections.Generic;
using System.Linq;
using WebShop.Discounts;

namespace WebShop.Baskets
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BasketId { get; set; }
        public bool IsDiscounted { get; set; }
        
        public Product Product { get; set; }
        public Basket Basket { get; set; }
        public IReadOnlyCollection<BasketItemDiscount> BasketItemDiscounts { get; set; }

        public IEnumerable<Discount> Discounts => BasketItemDiscounts.Select(bd => bd.Discount);

        /// <summary>  
        ///  Needs to be set after read from DB
        /// </summary> 
        public decimal Price { get; set; }
    }
}
