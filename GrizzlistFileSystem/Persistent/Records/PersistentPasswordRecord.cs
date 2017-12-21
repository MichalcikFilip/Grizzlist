using Grizzlist.Records;
using System;

namespace Grizzlist.FileSystem.Persistent.Records
{
    [Serializable]
    class PersistentPasswordRecord : PersistentBaseRecord, IConvertible<PasswordRecord, PersistentPasswordRecord, long>
    {
        public string Note { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public PasswordRecord Convert()
        {
            return new PasswordRecord() { ID = ID, Name = Name, Removed = Removed, Note = Note, User = User, Password = Password };
        }

        public PersistentPasswordRecord Convert(PasswordRecord entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                Name = entity.Name;
                Removed = entity.Removed;
                Note = entity.Note;
                User = entity.User;
                Password = entity.Password;
            }

            return this;
        }
    }
}
