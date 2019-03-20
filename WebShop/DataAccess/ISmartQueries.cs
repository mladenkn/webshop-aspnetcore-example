﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.DataAccess
{
    public interface ISmartQueries
    {
        //Task<IReadOnlyCollection<Discount>> GetDiscountsFor(Basket basket);
        Task<IReadOnlyCollection<Discount>> GetPossibleDiscountsFor(Basket basket);
    }
}
