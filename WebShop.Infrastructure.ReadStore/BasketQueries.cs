using System;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Baskets;

namespace WebShop.Infrastructure.ReadStore
{
    public class BasketQueries
    {
        private readonly IDataSyncJobsQueue _jobs;

        public BasketQueries(IDataSyncJobsQueue jobs)
        {
            _jobs = jobs;
        }

        public async Task<Basket> GetBasketWithDiscountsApplied(int basketId)
        {
            Task<Basket> GetActual()
            {
                throw new NotImplementedException();
            }

            var job = _jobs.Current.OfType<AddItemToBasketJob>().FirstOrDefault(j => j.BasketId == basketId);

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
