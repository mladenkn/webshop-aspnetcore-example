using System.Threading.Tasks;
using MediatR;
using WebShop.Models;

namespace WebShop
{
    public class BasketPriceRequested : INotification
    {
        public BasketPriceRequested(Basket basket)
        {
            Basket = basket;
        }

        public Basket Basket { get; }
    }
}
