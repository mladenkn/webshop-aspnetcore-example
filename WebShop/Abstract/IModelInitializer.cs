using System.Threading.Tasks;

namespace WebShop.Abstract
{
    public interface IModelInitializer<in TModel>
    {
        void Initialize(TModel model);
    }

    public interface IAsyncModelInitializer<in TModel>
    {
        Task Initialize(TModel model);
    }
}
