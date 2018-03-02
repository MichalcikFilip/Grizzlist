using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;

namespace Grizzlist.Client.Tasks.Drawings
{
    /// <summary>
    /// Interaction logic for DrawingEditorControl.xaml
    /// </summary>
    public partial class DrawingEditorControl : ValidatableControl
    {
        public DrawingEditorControl()
        {
            InitializeComponent();

            AddValidator(new EmptyStringValidator(tbName));
        }
    }
}
