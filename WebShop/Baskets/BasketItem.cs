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
        public IReadOnlyCollection<BasketItemDiscount> BasketItemDiscount { get; set; }

        public IEnumerable<Discount> Discounts => BasketItemDiscount.Select(bd => bd.Discount);

        /// <summary>  
        ///  Needs to be set after read from DB
        /// </summary> 
        public decimal Price { get; set; }
    }
}
