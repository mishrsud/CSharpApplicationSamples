using System;
using System.Collections.Generic;

namespace FakeBusinessLogic
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly DapperConnectionManager _dapperConnectionManager;

        protected Repository(DapperConnectionManager dapperConnectionManager)
        {
            _dapperConnectionManager = dapperConnectionManager;
        }

        public abstract void Save(T entity);

        public abstract T GetById(Guid id);

        public abstract IEnumerable<T> GetAllById(Guid id);
    }
}
