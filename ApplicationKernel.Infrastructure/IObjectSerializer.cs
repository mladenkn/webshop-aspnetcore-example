using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace ApplicationKernel.Infrastructure
{
    public interface IObjectSerializer<T>
    {
        IObjectSerializer<T> IgnoreProperty(Expression<Func<T, object>> property);
        IObjectSerializer<T> IgnoreProperty(string property);
        JObject Serialize();
    }

    public class ObjectSerializer<T> : IObjectSerializer<T>
    {
        public IObjectSerializer<T> IgnoreProperty(Expression<Func<T, object>> property)
        {
            throw new NotImplementedException();
        }

        public JObject Serialize()
        {
            throw new NotImplementedException();
        }
    }
}