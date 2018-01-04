using System.Collections.Generic;
using System.Windows;

namespace Grizzlist.Client.Collection
{
    /// <summary>
    /// Interaction logic for CollectionWindow.xaml
    /// </summary>
    public partial class CollectionWindow : Window
    {
        private ICollectionManager manager;
        private List<CollectionItem> items = new List<CollectionItem>();

        public CollectionWindow(Window owner, string title, ICollectionManager collectionManager)
        {
            Owner = owner;

            InitializeComponent();

            Title = title;
            manager = collectionManager;
            pnlScroll.MouseLeftButtonUp += (sender, e) => Deselect();

            foreach (CollectionItem item in manager.LoadItems())
            {
                item.Selected += () => Deselect();

                pnlContent.Children.Add(item);
                items.Add(item);
            }
        }

        private void Deselect()
        {
            items.ForEach(x => x.Deselect());
        }
    }
}
