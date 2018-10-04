using System;
using System.Threading.Tasks;
using ApplicationKernel;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Rest.Controllers
{
    public class BaseController
    {
        // Not gonna implement this
        // I have implemented this in another project, here's a link if you wanna see: https://github.com/mladenkn/ddd-cqrs-example
        protected Task<IActionResult> Handle(IRequest request) => throw new NotImplementedException();
    }
}
