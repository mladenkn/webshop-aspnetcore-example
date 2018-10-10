using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using MediatR;

namespace WebShop
{
    public static class Utilities
    {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (var value in values)
                action(value);
        }

        public static void AddTo<T>(this T o, ICollection<T> collection)
        {
            collection.Add(o);
        }

        public static Task PublishWith<T>(this T notification, IMediator mediator)
            where T : INotification
        {
            return mediator.Publish(notification);
        }

        public static Task WhenAll(this IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks);
        }
    }
}
