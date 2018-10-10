using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace WebShop.Abstract
{
    public interface IDatabaseTransaction
    {
        IDatabaseTransaction Save(object o);
        IDatabaseTransaction Update(object o);
        IDatabaseTransaction Delete(object o);
        Task Commit();
    }

    public class DatabaseTransactionExtensions
    {
        public IDatabaseTransaction UpdateRange(IDatabaseTransaction transaction, IEnumerable<object> objects)
        {
            objects.ForEach(o => transaction.Update(o));
            return transaction;
        }
    }

    public delegate IDatabaseTransaction NewTransaction();
}
