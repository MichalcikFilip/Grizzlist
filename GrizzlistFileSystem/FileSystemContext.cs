using Grizzlist.FileSystem.Persistent.Notifications;
using Grizzlist.FileSystem.Persistent.Records;
using Grizzlist.FileSystem.Persistent.Stats;
using Grizzlist.FileSystem.Persistent.Tasks;
using Grizzlist.FileSystem.Persistent.Tasks.Templates;
using Grizzlist.FileSystem.Repositories;
using Grizzlist.FileSystem.Serialization;
using Grizzlist.Notifications;
using Grizzlist.Persistent;
using Grizzlist.Records;
using Grizzlist.Stats;
using Grizzlist.Tasks;
using Grizzlist.Tasks.Templates;
using System;
using System.Collections.Generic;

namespace Grizzlist.FileSystem
{
    public class FileSystemContext : IPersistentContext
    {
        private static Dictionary<Type, Func<object>> repositoriesMapping = new Dictionary<Type, Func<object>>();

        public static Dictionary<Type, Func<object>> RepositoriesMapping { get { return repositoriesMapping; } }

        public static void Configure(string appDataFolder)
        {
            repositoriesMapping.Add(typeof(StatsManager), () => new DefaultRepository<StatsManager, PersistentStatsManager, long>(new DefaultSerializer<List<PersistentStatsManager>>($@"{appDataFolder}\Data\stats.dat")));
            repositoriesMapping.Add(typeof(Notification), () => new DefaultRepository<Notification, PersistentNotification, long>(new DefaultSerializer<List<PersistentNotification>>($@"{appDataFolder}\Data\notifications.dat")));
            repositoriesMapping.Add(typeof(Group<LinkRecord>), () => new DefaultRepository<Group<LinkRecord>, PersistentGroup<LinkRecord, PersistentLinkRecord>, long>(new DefaultSerializer<List<PersistentGroup<LinkRecord, PersistentLinkRecord>>>($@"{appDataFolder}\Data\links.dat")));
            repositoriesMapping.Add(typeof(Group<NoteRecord>), () => new DefaultRepository<Group<NoteRecord>, PersistentGroup<NoteRecord, PersistentNoteRecord>, long>(new DefaultSerializer<List<PersistentGroup<NoteRecord, PersistentNoteRecord>>>($@"{appDataFolder}\Data\notes.dat")));
            repositoriesMapping.Add(typeof(Group<PasswordRecord>), () => new DefaultRepository<Group<PasswordRecord>, PersistentGroup<PasswordRecord, PersistentPasswordRecord>, long>(new DefaultSerializer<List<PersistentGroup<PasswordRecord, PersistentPasswordRecord>>>($@"{appDataFolder}\Data\passwords.dat")));
            repositoriesMapping.Add(typeof(Task), () => new DefaultRepository<Task, PersistentTask, long>(new DefaultSerializer<List<PersistentTask>>($@"{appDataFolder}\Data\tasks.dat")));
            repositoriesMapping.Add(typeof(Template), () => new DefaultRepository<Template, PersistentTemplate, long>(new DefaultSerializer<List<PersistentTemplate>>($@"{appDataFolder}\Data\templates.dat")));
        }

        public IRepository<T, K> GetRepository<T, K>() where T : IPersistentEntity<K>
        {
            Type entityType = typeof(T);

            if (repositoriesMapping.ContainsKey(entityType))
                return repositoriesMapping[entityType]() as IRepository<T, K>;

            return null;
        }
    }
}
