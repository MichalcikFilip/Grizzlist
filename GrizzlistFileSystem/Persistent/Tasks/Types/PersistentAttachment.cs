using Grizzlist.Persistent;
using Grizzlist.Tasks.Types;
using System;

namespace Grizzlist.FileSystem.Persistent.Tasks.Types
{
    [Serializable]
    struct PersistentAttachment : IPersistentEntity<long>, IConvertible<Attachment, PersistentAttachment, long>, IIncrementalKey<long>
    {
        public long ID { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }

        public Attachment Convert()
        {
            return new Attachment() { ID = ID, Path = Path, Note = Note };
        }

        public PersistentAttachment Convert(Attachment entity)
        {
            ID = entity.ID;
            Path = entity.Path;
            Note = entity.Note;

            return this;
        }

        public bool CanAutoIncrement()
        {
            return true;
        }

        public long GetNewKey(long lastKey)
        {
            return ++lastKey;
        }
    }
}
