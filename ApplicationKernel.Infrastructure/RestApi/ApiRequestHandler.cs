using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IRequest = ApplicationKernel.Domain.MediatorSystem.IRequest;

namespace ApplicationKernel.Infrastructure.RestApi
{
    public class ApiRequestHandler
    {
        public async Task<IActionResult> Handle(IRequest request)
        {
            /*
             * should:
             * 1. validatie request
             * 2. handle with appropiate handler
             * 3. map to handler response to IActionResult
             */

            throw new NotImplementedException();
        }
    }

    public delegate Task<IActionResult> HandleApiRequest(IRequest request);
}
