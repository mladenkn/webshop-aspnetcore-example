using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.UseCases.Abstract
{
    public interface IRequest : MediatR.IRequest<Response>
    {
        
    }
}
