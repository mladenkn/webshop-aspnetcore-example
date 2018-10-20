using System.Collections.Generic;

namespace WebShop.Infrastructure.ReadStore
{
    public interface IDataSyncJobsQueue
    {
        IReadOnlyList<IDataSyncJob> Current { get; }
        void Add(IDataSyncJob job);
    }

    public class DataSyncJobsQueue : IDataSyncJobsQueue
    {
        private readonly List<IDataSyncJob> _current = new List<IDataSyncJob>();

        public IReadOnlyList<IDataSyncJob> Current => _current;

        public void Add(IDataSyncJob job) => _current.Add(job);
    }
}
