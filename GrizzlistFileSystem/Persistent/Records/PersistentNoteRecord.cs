using Grizzlist.Records;
using System;

namespace Grizzlist.FileSystem.Persistent.Records
{
    [Serializable]
    class PersistentNoteRecord : PersistentBaseRecord, IConvertible<NoteRecord, PersistentNoteRecord, long>
    {
        public string Note { get; set; }

        public NoteRecord Convert()
        {
            return new NoteRecord() { ID = ID, Name = Name, Removed = Removed, Note = Note };
        }

        public PersistentNoteRecord Convert(NoteRecord entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                Name = entity.Name;
                Removed = entity.Removed;
                Note = entity.Note;
            }

            return this;
        }
    }
}
