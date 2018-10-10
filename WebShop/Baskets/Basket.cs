using System.Collections.Generic;

namespace WebShop.Baskets
{
    public class Basket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IReadOnlyCollection<BasketItem> Items { get; set; }

        public User User { get; set; }
        
        /// <summary>  
        ///  Needs to be set after read from DB
        /// </summary> 
        public decimal TotalPrice { get; set; }
    }
}
