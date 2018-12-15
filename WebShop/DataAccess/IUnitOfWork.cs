using System.Threading.Tasks;

namespace WebShop.DataAccess
{
    public interface IUnitOfWork
    {
        void Add(object o);
        void Update(object o);
        void Delete(object o);
        Task PersistChanges();
    }
}
