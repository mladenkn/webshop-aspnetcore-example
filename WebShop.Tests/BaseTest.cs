using Bogus;

namespace WebShop.Tests
{
    public class BaseTest
    {
        protected static Faker<T> FakerOf<T>() where T : class => new Faker<T>();
    }
}
