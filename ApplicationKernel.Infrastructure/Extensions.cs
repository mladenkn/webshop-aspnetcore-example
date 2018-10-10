using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ApplicationKernel.Infrastructure
{
    public static class Extensions
    {
        public static OkObjectResult WrapIntoOkObjectResult(this JObject jObject)
        {
            return new OkObjectResult(jObject);
        }
    }
}
