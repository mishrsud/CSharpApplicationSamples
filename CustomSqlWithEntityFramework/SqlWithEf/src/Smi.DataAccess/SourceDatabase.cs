using System.Collections.Generic;
using System.Data.Entity;

namespace Smi.DataAccess
{
    public class SourceDatabase : IDatabase
    {
        private readonly Database _database;

        public SourceDatabase(Database database)
        {
            _database = database;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return _database.SqlQuery<TElement>(sql, parameters);
        }
    }
}