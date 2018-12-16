using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Tests
{
    internal static class Utils
    {
        internal static async Task PersistAll(this DbContext db, IEnumerable<object> objects)
        {
            foreach (var o in objects)
            {
                if (o is IEnumerable col)
                {
                    foreach (var o1 in col)
                        db.Add(o1);
                }
                else
                    db.Add(o);
            }

            await db.SaveChangesAsync();
        }

        internal static Task PersistAll(this DbContext db, params object[] objects)
        {
            return db.PersistAll((IEnumerable<object>) objects);
        }
    }
}
