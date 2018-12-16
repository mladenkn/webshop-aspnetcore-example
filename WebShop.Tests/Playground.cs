using System.Linq;
using FluentAssertions;
using WebShop.Models;
using Xunit;

namespace WebShop.Tests
{
    public class Playground
    {
        [Fact]
        public void Play()
        {
            var db = TestServiceFactory.Database();
            db.Users.Add(new User());
            db.SaveChangesAsync().Wait();
            db.Users.Count().Should().Be(1);
        }
    }
}
