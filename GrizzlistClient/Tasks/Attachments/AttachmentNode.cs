using Grizzlist.Logger;
using Grizzlist.Tasks.Types;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks.Attachments
{
    class AttachmentNode : TreeViewItem
    {
        private Image imgNote;

        public Attachment Attachment { get; set; }
        public string NodeName { get; set; }
        public string FullPath { get; set; }
        public bool IsFile { get; set; }

        public AttachmentNode(Attachment attachment, string nodeName, string fullPath, bool isFile)
        {
            Attachment = attachment;
            NodeName = nodeName;
            FullPath = fullPath;
            IsFile = isFile;

            Grid grid = new Grid() { Margin = new Thickness(2) };
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            grid.Children.Add(new Image() { Width = 16, Height = 16, Margin = new Thickness(0, 0, 4, 0), Source = new BitmapImage(new Uri($"/Resources/{GetIcon()}", UriKind.Relative)) });

            imgNote = new Image() { Width = 16, Height = 16, Margin = new Thickness(0, 0, 4, 0), Source = new BitmapImage(new Uri("/Resources/note.png", UriKind.Relative)), Visibility = Visibility.Collapsed };

            grid.Children.Add(imgNote);
            Grid.SetColumn(imgNote, 1);

            if (!string.IsNullOrWhiteSpace(attachment.Note) && IsFile)
            {
                imgNote.Visibility = Visibility.Visible;
                ToolTip = Attachment.Note;
            }

            TextBlock tbNodeName = new TextBlock() { Text = NodeName };

            grid.Children.Add(tbNodeName);
            Grid.SetColumn(tbNodeName, 2);

            Header = grid;

            if (IsFile && File.Exists(Attachment.Path))
                MouseDoubleClick += (s, e) =>
                {
                    if (e.ChangedButton == MouseButton.Left)
                    {
                        try
                        {
                            Process.Start(Attachment.Path);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message, this);
                        }
                    }
                };
        }

        public void UpdateNote(string note)
        {
            Attachment = new Attachment() { Path = Attachment.Path, Note = note };

            if (!string.IsNullOrWhiteSpace(note) && IsFile)
            {
                imgNote.Visibility = Visibility.Visible;
                ToolTip = note;
            }
            else
            {
                imgNote.Visibility = Visibility.Collapsed;
                ToolTip = null;
            }
        }

        private string GetIcon()
        {
            if (IsFile)
                return File.Exists(Attachment.Path) ? "fileIcon.png" : "fileDeleted.png";

            return "folder.png";
        }
    }
}
