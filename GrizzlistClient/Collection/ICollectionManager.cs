using System.Collections.Generic;

namespace Grizzlist.Client.Collection
{
    public interface ICollectionManager
    {
        IEnumerable<CollectionItem> LoadItems();
    }
}
