using System.Collections.Generic;
using WebShop.Discounts;

namespace WebShop.Baskets
{
    public class Basket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IReadOnlyCollection<BasketItem> Items { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public User User { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>  
        public IReadOnlyCollection<GrantedDiscount> GrantedDiscounts { get; set; }
    }
}
