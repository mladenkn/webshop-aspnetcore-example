using System.Linq;
using System.Threading.Tasks;
using WebShop.Baskets;
using WebShop.Infrastructure.ReadStore.Refreshing;

namespace WebShop.Infrastructure.ReadStore
{
    public class Queries
    {
        private readonly IDataRefreshJobsQueue _jobs;
        private readonly IReadStore _readStore;

        public Queries(IDataRefreshJobsQueue jobs, IReadStore readStore)
        {
            _jobs = jobs;
            _readStore = readStore;
        }

        public async Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            var job = _jobs.Current.OfType<RefreshBasketWithItemJob>().FirstOrDefault(j => j.BasketId == basketId);

            if (job == null)
                return await _readStore.GetBasket(basketId);
            else
            {
                await job.Task;
                return await _readStore.GetBasket(basketId);
            }
        }
    }
}
