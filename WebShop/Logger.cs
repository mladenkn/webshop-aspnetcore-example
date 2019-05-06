using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WebShop
{
    public class Logger : INotificationHandler<BasketPriceRequested>
    {
        public Task Handle(BasketPriceRequested notification, CancellationToken cancellationToken)
        {
            var basket = notification.Basket;
            Console.WriteLine($"Calculated price of basket of id: {basket.BasketId}. Price: {basket.Price}");
            return Task.CompletedTask;
        }
    }
}
