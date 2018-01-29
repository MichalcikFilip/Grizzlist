using System.Windows;
using System.Windows.Controls;

namespace Grizzlist.Client.Validators.BasicValidators
{
    class IntegerValidator : Validator
    {
        public IntegerValidator(TextBox control)
            : base(control)
        { }

        protected override bool Validate(Control control)
        {
            long integer;
            TextBox textBox = (TextBox)control;

            if (!long.TryParse(textBox.Text, out integer))
            {
                textBox.Style = resource["ErrorStyleControl"] as Style;
                textBox.ToolTip = "This field must be a number.";

                return false;
            }

            return true;
        }
    }
}
