using System;
using System.Collections.Generic;

namespace FakeBusinessLogic
{
    public interface IRepository<T> where T : class
    {
        void Save(T entity);
        T GetById(Guid id);
        IEnumerable<T> GetAllById(Guid id);
    }
}
