using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
                item.DoubleClick += () => Update_Click(null, null);

                pnlContent.Children.Add(item);
                items.Add(item);
            }
        }

        private void Deselect()
        {
            items.ForEach(x => x.Deselect());
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            foreach (CollectionItem item in items)
                item.Visibility = string.IsNullOrWhiteSpace(tbSearch.Text) ? Visibility.Visible : (manager.MatchSearch(item, tbSearch.Text) ? Visibility.Visible : Visibility.Collapsed);

            pnlSearch.BorderBrush = new SolidColorBrush(string.IsNullOrWhiteSpace(tbSearch.Text) ? Color.FromRgb(171, 173, 179) : Colors.Green);
        }

        private void SearchClearClick(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            SearchClick(sender, e);
        }

        private void Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchClick(sender, e);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            CollectionItem item = manager.AddItem(this);

            if (item != null)
            {
                item.Selected += () => Deselect();
                item.DoubleClick += () => Update_Click(null, null);

                pnlContent.Children.Add(item);
                items.Add(item);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            CollectionItem item = items.FirstOrDefault(x => x.IsSelected);

            if (item != null)
                manager.UpdateItem(item, this);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            CollectionItem item = items.FirstOrDefault(x => x.IsSelected);

            if (item != null && manager.RemoveItem(item))
            {
                pnlContent.Children.Remove(item);
                items.Remove(item);
            }
        }
    }
}
