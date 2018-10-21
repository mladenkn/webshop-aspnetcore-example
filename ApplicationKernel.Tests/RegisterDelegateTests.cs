using System;
using System.Linq;
using Autofac;
using Xunit;

namespace ApplicationKernel.Tests
{
    public class RegisterDelegateTests
    {
        [Fact]
        public void Component_is_not_previously_registered() => Run(false);

        [Fact]
        public void Component_is_previously_registered() => Run(true);

        private void Run(bool shouldPreviouslyRegisterComponent)
        {
            var containerBuilder = new ContainerBuilder();
            
            if (shouldPreviouslyRegisterComponent)
                containerBuilder.RegisterType<NumberProvider>();
            containerBuilder.RegisterDelegate<NumberProvider, ProvideNumber>(c => c.Provide);
            
            using (var scope = containerBuilder.Build().BeginLifetimeScope())
            {
                var provide = scope.Resolve<ProvideNumber>();
                Assert.Equal(provide(), NumberProvider.ProvidedNumber);
            }
        }
    }

    public class NumberProvider
    {
        internal int Provide() => ProvidedNumber;
        internal static int ProvidedNumber => 3;
    }

    internal delegate int ProvideNumber();
}
