using Grizzlist.Client.Collection;
using System.Collections.Generic;

namespace Grizzlist.Client.Tasks.Templates
{
    class TemplatesCollection : ICollectionManager
    {
        public IEnumerable<CollectionItem> LoadItems()
        {
            return new List<CollectionItem>() { new CollectionItem(new TemplateItem()), new CollectionItem(new TemplateItem()), new CollectionItem(new TemplateItem()) };
        }
    }
}
