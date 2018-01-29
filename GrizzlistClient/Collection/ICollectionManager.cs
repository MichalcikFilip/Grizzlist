using System.Collections.Generic;
using System.Windows;

namespace Grizzlist.Client.Collection
{
    public interface ICollectionManager
    {
        IEnumerable<CollectionItem> LoadItems();
        CollectionItem AddItem(Window owner);
        void UpdateItem(CollectionItem item, Window owner);
        bool RemoveItem(CollectionItem item);
        bool MatchSearch(CollectionItem item, string text);
    }
}
