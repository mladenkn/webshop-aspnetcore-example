using System;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationKernel.Infrastructure.RestApi
{
    public abstract class ApiController
    {
        private readonly HandleApiRequest _handle;

        protected ApiController(HandleApiRequest handle)
        {
            _handle = handle;
        }

        protected Task<IActionResult> Handle(IRequest request) => _handle(request);
    }
}
