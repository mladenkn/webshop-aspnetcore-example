using System.Threading.Tasks;
using Xunit;

namespace WebShop.Tests.ApplyDiscountsTest
{
    public class Cases
    {
        [Fact]
        public Task Buy_1_bread___discount_1_butter() =>
            Runner.Run(new Arguments
            {
                NumberOfBreadsPurchased = 1,
                NumberOfButterPurchased = 1,
                RequiredQuantityOfBread = 1,
                NumberOfButtersThatShouldReceiveDiscount = 1
            });

        [Fact]
        public Task Buy_1_bread___discount_2_butters() =>
            Runner.Run(new Arguments
            {
                NumberOfBreadsPurchased = 1,
                NumberOfButterPurchased = 2,
                RequiredQuantityOfBread = 1,
                NumberOfButtersThatShouldReceiveDiscount = 1
            });

        [Fact]
        public Task Buy_2_bread___discount_1_butter() =>
            Runner.Run(new Arguments
            {
                NumberOfBreadsPurchased = 2,
                NumberOfButterPurchased = 1,
                RequiredQuantityOfBread = 2,
                NumberOfButtersThatShouldReceiveDiscount = 1
            });

        [Fact]
        public Task Buy_2_bread___discount_2_butters() =>
            Runner.Run(new Arguments
            {
                NumberOfBreadsPurchased = 2,
                NumberOfButterPurchased = 2,
                RequiredQuantityOfBread = 2,
                NumberOfButtersThatShouldReceiveDiscount = 2
            });

        [Fact]
        public Task Buy_2_bread___discount_1_bread() =>
            Runner.Run(new Arguments
            {
                NumberOfBreadsPurchased = 2,
                NumberOfButterPurchased = 1,
                RequiredQuantityOfBread = 2,
                NumberOfButtersThatShouldReceiveDiscount = 1
            });
    }
}
