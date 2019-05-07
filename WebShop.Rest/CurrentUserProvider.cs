using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebShop.Services;

namespace WebShop.Rest
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        public string GetId() => "mladen";
    }
}
