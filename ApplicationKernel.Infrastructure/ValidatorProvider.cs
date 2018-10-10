using System;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationKernel.Infrastructure
{
    public interface IValidatorProvider
    {
        IValidator ValidatorOf(Type type);
    }

    public class ValidatorProvider : IValidatorProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator ValidatorOf(Type type)
        {
            var validators = _serviceProvider.GetServices<IValidator>();
            var thisRequestValidator = validators.FirstOrDefault(it => it.CanValidateInstancesOfType(type));
            return thisRequestValidator;
        }
    }

}
