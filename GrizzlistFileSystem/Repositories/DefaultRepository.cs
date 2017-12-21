using Grizzlist.FileSystem.Persistent;
using Grizzlist.FileSystem.Serialization;
using Grizzlist.Persistent;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.FileSystem.Repositories
{
    public class DefaultRepository<T, D, K> : BaseRespository<List<D>>, IRepository<T, K> where T : IPersistentEntity<K> where D : IPersistentEntity<K>, IConvertible<T, D, K>, IIncrementalKey<K>, new ()
    {
        protected Dictionary<K, D> entities;

        public DefaultRepository(ISerializer<List<D>> serializer)
            : base(serializer)
        {
            entities = GetAllData()?.ToDictionary(x => x.ID, x => x) ?? new Dictionary<K, D>();
        }

        public virtual bool Add(T entity)
        {
            if (entity != null)
            {
                D persistentEntity = new D();

                if (persistentEntity.CanAutoIncrement())
                    entity.ID = persistentEntity.GetNewKey(entities.Count > 0 ? entities.Values.Max(x => x.ID) : default(K));

                if (!entities.ContainsKey(entity.ID))
                {
                    entities.Add(entity.ID, persistentEntity.Convert(entity));
                    return true;
                }
            }

            return false;
        }

        public virtual bool Update(T entity)
        {
            if (entity != null && entities.ContainsKey(entity.ID))
            {
                entities[entity.ID] = new D().Convert(entity);
                return true;
            }

            return false;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return entities.Values.Select(x => x.Convert()) ?? new List<T>();
        }

        protected virtual List<D> GetAllData()
        {
            return serializer?.Deserialize() ?? new List<D>();
        }

        public virtual T Get(T entity)
        {
            if (entity != null)
                return Get(entity.ID);

            return default(T);
        }

        public virtual T Get(K key)
        {
            if (entities.ContainsKey(key))
                return entities[key].Convert();

            return default(T);
        }

        public virtual bool Remove(T entity)
        {
            if (entity != null)
                return Remove(entity.ID);

            return false;
        }

        public virtual bool Remove(K key)
        {
            if (entities.ContainsKey(key))
            {
                entities.Remove(key);
                return true;
            }

            return false;
        }

        public virtual bool Save()
        {
            return serializer?.Serialize(entities.Values.ToList()) ?? false;
        }

        public virtual void Dispose()
        {
            Save();
        }
    }
}
