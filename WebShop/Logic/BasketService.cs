//using System.Linq;
//using System.Threading.Tasks;
//using MediatR;
//using Utilities;
//using WebShop.DataAccess;
//using WebShop.Models;

//namespace WebShop.Logic
//{
//    public interface IBasketService
//    {
//        Task<Basket> AddItem(Basket basket, int productId);
//    }

//    public class BasketService : IBasketService
//    {
//        private readonly IDiscountService _discountService;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly decimal _maxAllowedDiscount;
//        private readonly IMediator _mediator;

//        public BasketService(
//            IDiscountService discountService, IUnitOfWork unitOfWork, decimal maxAllowedDiscount, IMediator mediator)
//        {
//            _discountService = discountService;
//            _unitOfWork = unitOfWork;
//            _maxAllowedDiscount = maxAllowedDiscount;
//            _mediator = mediator;
//        }

//        public async Task<Basket> AddItem(Basket basket, int productId)
//        {
//            var basketItem = new BasketItem { BasketId = basket.Id, ProductId = productId };
//            basket.Items.Add(basketItem);

//            basketItem.Discounts = await _discountService.ApplyDiscounts(basket);

//            RefreshPriceOf(basketItem);
//            RefreshPriceOf(basket);

//            // todo: persist to db

//            return basket;
//        }

//        private void RefreshPriceOf(Basket basket)
//        {
//            basket.Items.Must().NotBeNull();
//            basket.Price = basket.Items.Sum(i => i.Price);
//            _mediator.Publish(new BasketPriceRequested(basket));
//        }

//        private void RefreshPriceOf(BasketItem item)
//        {
//            item.Discounts.Must().NotBeNull();
//            item.Product.Must().NotBeNull();

//            var totalDiscount = item.Discounts.Select(d => d.Discount.Value).Sum();
//            if (totalDiscount > _maxAllowedDiscount)
//                totalDiscount = _maxAllowedDiscount;

//            var without = item.Product.RegularPrice * totalDiscount;
//            item.Price = item.Product.RegularPrice - without;
//        }
//    }
//}
