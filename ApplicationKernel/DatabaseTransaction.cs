using System.Threading.Tasks;

namespace ApplicationKernel
{
    public interface IDatabaseTransaction
    {
        IDatabaseTransaction Save(object o);
        IDatabaseTransaction Update(object o);
        IDatabaseTransaction Delete(object o);
        Task Commit();
    }

    public delegate IDatabaseTransaction NewTransaction();
}
