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
        public event Action DoubleClick;

        public bool IsSelected { get; private set; }
        public UIElement ItemContent { get; private set; }

        public CollectionItem(UIElement content)
        {
            ItemContent = content;
            InitializeComponent();

            mainGrid.Children.Add(content);
            mainGrid.MouseLeftButtonUp += (sender, e) => e.Handled = true;
        }

        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected?.Invoke();

            IsSelected = true;
            mainGrid.Background = new SolidColorBrush(Color.FromRgb(216, 237, 255));

            if (e.ClickCount == 2)
                DoubleClick?.Invoke();
        }

        public void Deselect()
        {
            IsSelected = false;
            mainGrid.Background = new SolidColorBrush(Colors.White);
        }
    }
}
