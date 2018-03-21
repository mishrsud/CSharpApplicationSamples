using System.Collections.Generic;

namespace Smi.DataAccess
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllRecords();
    }
}