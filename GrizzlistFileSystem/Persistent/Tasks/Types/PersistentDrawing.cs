using Grizzlist.Persistent;
using Grizzlist.Tasks.Types;
using System;
using System.Drawing;

namespace Grizzlist.FileSystem.Persistent.Tasks.Types
{
    [Serializable]
    struct PersistentDrawing : IPersistentEntity<long>, IConvertible<Drawing, PersistentDrawing, long>, IIncrementalKey<long>
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Bitmap Image { get; set; }

        public Drawing Convert()
        {
            return new Drawing() { ID = ID, Name = Name, Note = Note, Image = Image };
        }

        public PersistentDrawing Convert(Drawing entity)
        {
            ID = entity.ID;
            Name = entity.Name;
            Note = entity.Note;
            Image = entity.Image;

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
