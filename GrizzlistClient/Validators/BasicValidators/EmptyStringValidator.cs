using System.Windows;
using System.Windows.Controls;

namespace Grizzlist.Client.Validators.BasicValidators
{
    class EmptyStringValidator : Validator
    {
        public EmptyStringValidator(TextBox control)
            : base(control)
        { }

        protected override bool Validate(Control control)
        {
            TextBox textBox = (TextBox)control;

            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Style = resource["ErrorStyleControl"] as Style;
                textBox.ToolTip = "This field cannot be empty.";

                return false;
            }

            return true;
        }
    }
}
