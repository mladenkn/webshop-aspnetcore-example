using System;
using System.Threading.Tasks;
using ApplicationKernel.MediatorSystem;
using Microsoft.AspNetCore.Mvc;
using IRequest = ApplicationKernel.MediatorSystem.IRequest;

namespace ApplicationKernel.Infrastructure.RestApi
{
    public interface IApiRequestHandler
    {
        Task<IActionResult> Handle(IRequest request);
        Task<Response> HandleWithoutMapping(IRequest request);
    }

    public class ApiRequestHandler : IApiRequestHandler
    {
        public Task<IActionResult> Handle(IRequest request)
        {
            /*
             * should:
             * 1. validate request
             * 2. handle with appropriate handler
             * 3. map to handler response to IActionResult
             */
            // you can see this implemented here:
                // https://github.com/mladenkn/ddd-cqrs-example/blob/master/ApplicationKernel.Infrastructure/WebApi/ApiRequestHandler.cs
            throw new NotImplementedException();
        }

        public Task<Response> HandleWithoutMapping(IRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
