using System;
using Autofac;

namespace ApplicationKernel
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterDelegate<T, TDelegate>(this ContainerBuilder container, Func<T, TDelegate> getDelegate)
        {
            container.RegisterType<T>(); // not sure if this is safe, what if it is already registered?
            container.Register(c => getDelegate(c.Resolve<T>()));
            return container;
        }
    }
}
