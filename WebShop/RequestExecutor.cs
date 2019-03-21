using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.DataAccess;
using WebShop.Logic;
using WebShop.Models;

namespace WebShop
{
    public interface IRequestExecutor
    {
        Task<Basket> AddItemToBasket(int productId);
    }

    public class RequestExecutor : IRequestExecutor
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IQueries _queries;

        public RequestExecutor(ICurrentUserProvider currentUserProvider, IQueries queries)
        {
            _currentUserProvider = currentUserProvider;
            _queries = queries;
        }

        public async Task<Basket> AddItemToBasket(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
