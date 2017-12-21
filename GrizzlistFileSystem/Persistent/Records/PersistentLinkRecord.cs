using Grizzlist.Records;
using System;

namespace Grizzlist.FileSystem.Persistent.Records
{
    [Serializable]
    class PersistentLinkRecord : PersistentBaseRecord, IConvertible<LinkRecord, PersistentLinkRecord, long>
    {
        public string Note { get; set; }
        public string Link { get; set; }

        public LinkRecord Convert()
        {
            return new LinkRecord() { ID = ID, Name = Name, Removed = Removed, Note = Note, Link = Link };
        }

        public PersistentLinkRecord Convert(LinkRecord entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                Name = entity.Name;
                Removed = entity.Removed;
                Note = entity.Note;
                Link = entity.Link;
            }

            return this;
        }
    }
}
