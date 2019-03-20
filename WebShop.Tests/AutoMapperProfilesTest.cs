using AutoMapper;
using WebShop.Infrastructure.DataAccess;
using Xunit;

namespace WebShop.Tests
{
    public class AutoMapperProfilesTest
    {
        [Fact]
        public void Run()
        {
            var config = new MapperConfiguration(c => { c.AddProfile<MapperProfile>(); });
            config.AssertConfigurationIsValid();
        }
    }
}
