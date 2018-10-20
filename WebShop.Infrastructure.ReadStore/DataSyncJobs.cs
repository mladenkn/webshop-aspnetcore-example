using System.Threading.Tasks;

namespace WebShop.Infrastructure.ReadStore
{
    public interface IDataSyncJob
    {
        
    }

    public class AddItemToBasketJob : IDataSyncJob
    {
        public Task Task { get; set; }
        public int BasketId { get; set; }
        public int BasketItemId { get; set; }
    }
}
