using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Grizzlist.Client.Collection
{
    /// <summary>
    /// Interaction logic for CollectionItem.xaml
    /// </summary>
    public partial class CollectionItem : UserControl
    {
        public event Action Selected;

        public bool IsSelected { get; private set; }

        public CollectionItem(UIElement content)
        {
            InitializeComponent();
            mainGrid.Children.Add(content);

            mainGrid.MouseLeftButtonUp += (sender, e) => e.Handled = true;
        }

        private void mainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected?.Invoke();

            IsSelected = true;
            mainGrid.Background = new SolidColorBrush(Color.FromRgb(216, 237, 255));
        }

        public void Deselect()
        {
            IsSelected = false;
            mainGrid.Background = new SolidColorBrush(Colors.White);
        }
    }
}
