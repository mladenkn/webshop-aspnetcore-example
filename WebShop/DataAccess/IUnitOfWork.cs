using System.Collections.Generic;
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

    public static class UnitOfWork
    {
        public static void AddRange(this IUnitOfWork unitOfWork, IEnumerable<object> objects)
        {
            foreach (var o in objects)
                unitOfWork.Add(o);
        }
    }
}
