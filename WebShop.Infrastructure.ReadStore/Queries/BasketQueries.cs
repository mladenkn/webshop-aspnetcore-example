using System;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Baskets;
using WebShop.Infrastructure.ReadStore.Refreshing;

namespace WebShop.Infrastructure.ReadStore.Queries
{
    public class BasketQueries
    {
        private readonly IDataRefreshJobsQueue _jobs;

        public BasketQueries(IDataRefreshJobsQueue jobs)
        {
            _jobs = jobs;
        }

        public async Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            Task<Basket> GetActual()
            {
                throw new NotImplementedException();
            }

            var job = _jobs.Current.OfType<RefreshBasketWithItemJob>().FirstOrDefault(j => j.BasketId == basketId);

            if (job == null)
                return await GetActual();
            else
            {
                await job.Task;
                return await GetActual();
            }
        }
    }
}
