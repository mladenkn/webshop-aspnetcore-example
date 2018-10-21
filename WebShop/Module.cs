using ApplicationKernel;
using Autofac;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            /*
             * TODO: add better extension to do this,
             *       so that is not needed to repeat the component name each time when registering new delegate of it
             */
            builder.RegisterDelegate<BasketItemService, CalculateBasketItemPrice>(c => c.CalculateItemPrice);
            builder.RegisterDelegate<BasketService, GetBasketWithDiscountsApplied>(c => c.GetBasketWithDiscountsApplied);
            builder.RegisterDelegate<DiscountService, AddDiscountsToBasketItem>(c => c.AddDiscountsToBasketItem);
        }
    }
}
