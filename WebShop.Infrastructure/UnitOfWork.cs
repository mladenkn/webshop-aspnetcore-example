using System;
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
                var (m1, m2) = _mapper.ToDbModels(discount);
                _dbContext.AddRange(m1);
                _dbContext.AddRange(m2);
            }
            else
                _dbContext.Add(o);
        }

        public void Update(object o)
        {
            if (o is Discount discount)
            {
                var (m1, m2) = _mapper.ToDbModels(discount);
                _dbContext.UpdateRange(m1);
                _dbContext.UpdateRange(m2);
            }
            else
                _dbContext.Update(o);
        }

        public void Delete(object o)
        {
            if (o is Discount discount)
            {
                var (m1, m2) = _mapper.ToDbModels(discount);
                _dbContext.RemoveRange(m1);
                _dbContext.RemoveRange(m2);
            }
            else
                _dbContext.Remove(o);
        }

        public Task PersistChanges() => _dbContext.SaveChangesAsync();
    }
}
