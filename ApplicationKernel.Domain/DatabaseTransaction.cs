﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace ApplicationKernel
{
    public interface IDatabaseTransaction
    {
        IDatabaseTransaction Save(object o);
        IDatabaseTransaction Update(object o);
        IDatabaseTransaction Delete(object o);
        Task Commit();
    }

    public static class DatabaseTransactionExtensions
    {
        public static IDatabaseTransaction UpdateRange(this IDatabaseTransaction transaction, IEnumerable<object> objects)
        {
            objects.ForEach(transaction.Update);
            return transaction;
        }

        public static IDatabaseTransaction SaveRange(this IDatabaseTransaction transaction, IEnumerable<object> objects)
        {
            objects.ForEach(transaction.Save);
            return transaction;
        }
    }

    public delegate IDatabaseTransaction NewTransaction();
}