using System;
using Autofac;

namespace ApplicationKernel
{
    public static class RegistrationBuilderExtensions
    {
        public static ContainerBuilder RegisterDelegate<T, TDelegate>(this ContainerBuilder container, Func<T, TDelegate> getDelegate)
        {
            container.RegisterType<T>();
            container.Register(c => getDelegate(c.Resolve<T>()));
            return container;
        }
    }
}
