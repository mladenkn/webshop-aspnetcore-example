using Autofac;

namespace WebShop.Infrastructure.PersistentCache
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasketsWithDiscountsCache>().As<BasketsWithDiscountsCache>();
        }
    }
}
