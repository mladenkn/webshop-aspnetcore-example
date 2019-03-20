using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebShopDbContext _dbContext;
        private readonly ICustomMapper _mapper;

        public UnitOfWork(WebShopDbContext dbContext, ICustomMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Add(object o)
        {
            if (o is Discount discount)
            {
                var (rps,  mds) = _mapper.ToDbModels(discount);
                _dbContext.AddRange(rps);
                _dbContext.AddRange(mds);
            }
            else
                _dbContext.Add(o);
        }

        public void Update(object o)
        {
            if (o is Discount)
                throw new NotImplementedException();
            else
                _dbContext.Update(o);
        }

        public void Delete(object o)
        {
            if (o is Discount discount)
            {
                var (rps, mds) = _mapper.ToDbModels(discount);
                _dbContext.RemoveRange(rps);
                _dbContext.RemoveRange(mds);
            }
            else
                _dbContext.Remove(o);
        }

        public Task PersistChanges() => _dbContext.SaveChangesAsync();
    }
}
