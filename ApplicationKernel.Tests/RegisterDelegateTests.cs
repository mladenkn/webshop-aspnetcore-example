using System;
using Autofac;
using Xunit;

namespace ApplicationKernel.Tests
{
    public class RegisterDelegateTests
    {
        [Fact]
        public void When_component_is_not_previously_registered() => Run(false);

        [Fact]
        public void When_component_is_previously_registered() => Run(true);

        private void Run(bool shouldPreviouslyRegisterComponent)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<NumberProvider>();
            if (shouldPreviouslyRegisterComponent)
                containerBuilder.RegisterType<NumberProviderWrapper>();
            containerBuilder.RegisterDelegate<NumberProviderWrapper, ProvideNumber>(c => c.Provide);

            using (var container = containerBuilder.Build().BeginLifetimeScope())
            {
                var provide = container.Resolve<ProvideNumber>();
                Assert.Equal(provide(), NumberProvider.ProvidedNumber);
            }
        }
    }

    public class NumberProvider
    {
        internal int Provide() => ProvidedNumber;
        internal static int ProvidedNumber => 3;
    }

    public class NumberProviderWrapper
    {
        private readonly NumberProvider _numberProvider;
        public NumberProviderWrapper(NumberProvider numberProvider)
        {
            _numberProvider = numberProvider;
        }
        public int Provide() => _numberProvider.Provide();
    }

    internal delegate int ProvideNumber();
}
