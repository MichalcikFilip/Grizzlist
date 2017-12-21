using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using Grizzlist.Tasks;
using System;
using System.Windows;

namespace Grizzlist.Client.Tasks
{
    /// <summary>
    /// Interaction logic for EditSubTaskWindow.xaml
    /// </summary>
    public partial class EditSubTaskWindow : ValidatableWindow
    {
        public SubTask EditedTask { get; private set; }

        public EditSubTaskWindow(Window owner, SubTask task = null)
        {
            Owner = owner;

            InitializeComponent();

            AddValidator(new EmptyStringValidator(tbName));

            if (task != null)
            {
                tbName.Text = task.Name;
                tbDescription.Text = task.Description;

                EditedTask = task;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (EditedTask != null)
                {
                    EditedTask.Name = tbName.Text;
                    EditedTask.Description = tbDescription.Text;
                    EditedTask.Updated = DateTime.Now;
                }
                else
                {
                    EditedTask = new SubTask(tbName.Text, tbDescription.Text);
                }

                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
