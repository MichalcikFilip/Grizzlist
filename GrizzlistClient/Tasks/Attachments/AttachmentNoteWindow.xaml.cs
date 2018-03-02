using System.Windows;

namespace Grizzlist.Client.Tasks.Attachments
{
    /// <summary>
    /// Interaction logic for AttachmentNoteWindow.xaml
    /// </summary>
    public partial class AttachmentNoteWindow : Window
    {
        public string Note
        {
            get { return tbNote.Text; }
            set { tbNote.Text = value; }
        }

        public AttachmentNoteWindow(Window owner)
        {
            Owner = owner;
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
