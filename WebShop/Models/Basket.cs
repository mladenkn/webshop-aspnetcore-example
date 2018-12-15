using System.Collections.Generic;
using System.Linq;

namespace WebShop.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<BasketItem> Items { get; set; }
        public decimal Price { get; set; }

        public User User { get; set; }
    }
}
