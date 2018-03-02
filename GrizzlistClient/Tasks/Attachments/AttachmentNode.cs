using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks.Attachments
{
    class AttachmentNode : TreeViewItem
    {
        public string NodeName { get; set; }

        public AttachmentNode(string nodeName)
        {
            NodeName = nodeName;

            Grid grid = new Grid() { Margin = new Thickness(2) };
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            grid.Children.Add(new Image() { Width = 16, Height = 16, Margin = new Thickness(0, 0, 4, 0), Source = new BitmapImage(new Uri("/Resources/folder.png", UriKind.Relative)) });

            Image note = new Image() { Width = 16, Height = 16, Margin = new Thickness(0, 0, 4, 0), Source = new BitmapImage(new Uri("/Resources/note.png", UriKind.Relative)) };
            grid.Children.Add(note);

            Grid.SetColumn(note, 1);

            TextBlock tbName = new TextBlock() { Text = nodeName };
            grid.Children.Add(tbName);
            Grid.SetColumn(tbName, 2);

            Header = grid;
        }
    }
}
