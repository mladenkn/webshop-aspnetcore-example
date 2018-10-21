using System.Threading.Tasks;
using Xunit;

namespace WebShop.Tests.GetBasketWithDiscountsAppliedTest
{
    public class Cases
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly DataContainer _data = new DataContainer();

        [Fact]
        public async Task Basket_has___1_bread__1_butter__1_milk()
        {
            await _fixture
                .WithBasketItemOf(_data.Butter)
                .WithBasketItemOf(_data.Bread)
                .WithBasketItemOf(_data.Milk)
                .BasketPriceShouldBe(2.95m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___2_butters__2_breads()
        {
            await _fixture
                .WithDiscounts(_data.BreadDiscount)
                .WithBasketItemsOf(_data.Butter, 2)
                .WithBasketItemsOf(_data.Bread, 2)
                .BasketPriceShouldBe(3.1m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___4_milks()
        {
            await _fixture
                .WithDiscounts(_data.MilkDiscount)
                .WithBasketItemsOf(_data.Milk, 4)
                .BasketPriceShouldBe(3.45m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___2_butter__1_bread__8_milk()
        {
            await _fixture
                .WithDiscounts(_data.MilkDiscount, _data.BreadDiscount)
                .WithBasketItemsOf(_data.Butter, 2)
                .WithBasketItemOf(_data.Bread)
                .WithBasketItemsOf(_data.Milk, 8)
                .BasketPriceShouldBe(9)
                .Run();
        }
    }
}
