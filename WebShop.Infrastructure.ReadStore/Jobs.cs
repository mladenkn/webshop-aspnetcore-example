using ApplicationKernel.Domain;

namespace WebShop.Infrastructure.PersistentCache
{
    internal interface IBasketCacheJob : IJob
    {
        int BasketId { get; }
    }
}
