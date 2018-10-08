using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace WebShop.Tests.Abstract
{
    public class FakerContainer : IFakerContainer
    {
        private readonly ICollection<object> _fakers = new List<object>();

        public Faker<T> FakerOf<T>() where T : class
        {
            var faker = _fakers.OfType<Faker<T>>().FirstOrDefault();
            if (faker == null)
            {
                faker = new Faker<T>();
                _fakers.Add(faker);
            }
            return faker;
        }
    }

    public interface IFakerContainer
    {
        Faker<T> FakerOf<T>() where T : class;
    }
}
