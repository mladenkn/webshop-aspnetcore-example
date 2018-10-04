using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.UseCases.Abstract;

namespace WebShop.Rest.Controllers
{
    public class BaseController
    {
        protected Task<IActionResult> Handle(IRequest request) => throw new NotImplementedException();
    }
}
