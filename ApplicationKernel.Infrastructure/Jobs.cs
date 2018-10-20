using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationKernel.Infrastructure
{
    public interface IJobQueue
    {
        IReadOnlyList<IJob> Current { get; }
        void Add(IJob job);
    }

    public class JobQueue : IJobQueue
    {
        private readonly List<IJob> _current = new List<IJob>();

        public IReadOnlyList<IJob> Current => _current;

        public void Add(IJob job)
        {
            _current.Add(job);
            job.Task.ContinueWith(t => _current.Remove(job));
        }
    }

    public interface IJob
    {
        Task Task { get; set; }
    }
}
