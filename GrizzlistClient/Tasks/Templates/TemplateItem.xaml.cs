using Grizzlist.Tasks.Templates;
using System.Windows.Controls;

namespace Grizzlist.Client.Tasks.Templates
{
    /// <summary>
    /// Interaction logic for TemplateItem.xaml
    /// </summary>
    public partial class TemplateItem : UserControl
    {
        public Template TaskTemplate { get; private set; }

        public TemplateItem(Template template)
        {
            TaskTemplate = template;

            InitializeComponent();
            Update();
        }

        public void Update()
        {
            tbName.Text = TaskTemplate.Task.Name;
            tbDescription.Text = TaskTemplate.Task.Description;
        }
    }
}
