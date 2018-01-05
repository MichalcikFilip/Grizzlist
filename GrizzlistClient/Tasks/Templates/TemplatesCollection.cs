using Grizzlist.Client.Collection;
using Grizzlist.Client.Persistent;
using Grizzlist.Persistent;
using Grizzlist.Tasks.Templates;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.Client.Tasks.Templates
{
    class TemplatesCollection : ICollectionManager
    {
        public IEnumerable<CollectionItem> LoadItems()
        {
            using (IRepository<Template, long> repository = PersistentFactory.GetContext().GetRepository<Template, long>())
                return repository.GetAll().Select(x => new CollectionItem(new TemplateItem(x)));
        }
    }
}
