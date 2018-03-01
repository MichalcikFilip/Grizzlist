using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Grizzlist.FileSystem.Serialization
{
    public class DefaultSerializer<T> : BaseSerializer<T>
    {
        public DefaultSerializer(string filePath)
            : base($@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{filePath}")
        { }

        public override T Deserialize()
        {
            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
                using (FileStream reader = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    return (T)new BinaryFormatter().Deserialize(reader);

            return default(T);
        }

        public override bool Serialize(T entity)
        {
            if (!string.IsNullOrEmpty(FilePath) && typeof(T).IsSerializable)
            {
                string directoryPath = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                using (FileStream writer = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    new BinaryFormatter().Serialize(writer, entity);

                return true;
            }

            return false;
        }
    }
}
