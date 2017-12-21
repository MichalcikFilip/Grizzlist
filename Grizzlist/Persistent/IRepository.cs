using System;
using System.Collections.Generic;

namespace Grizzlist.Persistent
{
    public interface IRepository<T, K> : IDisposable where T : IPersistentEntity<K>
    {
        bool Add(T entity);
        bool Update(T entity);
        IEnumerable<T> GetAll();
        T Get(T entity);
        T Get(K key);
        bool Remove(T entity);
        bool Remove(K key);
        bool Save();
    }
}
