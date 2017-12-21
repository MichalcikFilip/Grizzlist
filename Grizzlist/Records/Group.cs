using Grizzlist.Persistent;
using System.Collections.Generic;

namespace Grizzlist.Records
{
    public class Group<T> : PersistentEntity<long> where T : BaseRecord
    {
        private List<T> records = new List<T>();

        public string Name { get; set; }
        public List<T> Records { get { return records; } }
    }
}
