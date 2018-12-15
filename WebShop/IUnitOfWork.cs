using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    public interface IUnitOfWork
    {
        void Add(object o);
        void Update(object o);
        void Delete(object o);
        Task PersistChanges();
    }
}
