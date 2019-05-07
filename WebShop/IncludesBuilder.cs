using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebShop
{
    public class IncludesBuilder<TModel>
    {
        public IReadOnlyCollection<string> Includes { get; } = new List<string>();

        public IncludesBuilder<TModel> Add(Expression<Func<TModel, object>> exp)
        {
            var expBodyString = exp.Body.ToString();
            var indexOfFirstDot = expBodyString.IndexOf(".");
            var propName = expBodyString.Substring(indexOfFirstDot + 1);
            ((List<string>)Includes).Add(propName);
            return this;
        }

        public IncludesBuilder<TModel> Add(string propName)
        {
            ((List<string>)Includes).Add(propName);
            return this;
        }
    }
}
