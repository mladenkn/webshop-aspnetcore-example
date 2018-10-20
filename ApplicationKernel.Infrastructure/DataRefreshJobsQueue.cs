using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationKernel.Infrastructure
{
    public interface IDataRefreshJobsQueue
    {
        IReadOnlyList<IDataRefreshJob> Current { get; }
        void Add(IDataRefreshJob job);
    }

    public class DataRefreshJobsQueue : IDataRefreshJobsQueue
    {
        private readonly List<IDataRefreshJob> _current = new List<IDataRefreshJob>();

        public IReadOnlyList<IDataRefreshJob> Current => _current;

        public void Add(IDataRefreshJob job)
        {
            _current.Add(job);
            job.Task.ContinueWith(t => _current.Remove(job));
        }
    }

    public interface IDataRefreshJob
    {
        Task Task { get; set; }
    }
}
