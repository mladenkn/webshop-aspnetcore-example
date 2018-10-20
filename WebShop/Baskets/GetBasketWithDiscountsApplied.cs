using System.Threading.Tasks;

namespace WebShop.Baskets
{
    public delegate Task<Basket> GetBasketWithDiscountsApplied(int basketId);
}