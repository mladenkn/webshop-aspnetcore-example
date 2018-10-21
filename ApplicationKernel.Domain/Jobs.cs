using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationKernel
{
    public interface IJobQueue
    {
        IReadOnlyList<IJob> Jobs { get; }
        void Add(IJob job);
    }

    public class JobQueue : IJobQueue
    {
        private readonly List<IJob> _current = new List<IJob>();

        public IReadOnlyList<IJob> Jobs => _current;

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
