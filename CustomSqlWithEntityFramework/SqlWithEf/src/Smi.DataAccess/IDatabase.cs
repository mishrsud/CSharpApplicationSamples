using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Smi.DataAccess
{
    public interface IDatabase
    {
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);
    }
}
