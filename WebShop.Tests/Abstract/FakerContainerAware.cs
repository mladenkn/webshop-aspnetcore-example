using Bogus;

namespace WebShop.Tests.Abstract
{
    public class FakerContainerAware
    {
        private readonly IFakerContainer _fakerContainer = new FakerContainer();
        protected Faker<T> FakerOf<T>() where T : class => _fakerContainer.FakerOf<T>();
    }
}
