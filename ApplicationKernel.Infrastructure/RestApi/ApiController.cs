using System;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationKernel.Infrastructure.RestApi
{
    public abstract class ApiController
    {
        protected Task<IActionResult> Handle(IRequest request)
        {
            throw new NotImplementedException();
        }

        protected IObjectSerializer<T> SerializerOf<T>() => new ObjectSerializer<T>();
    }
}
