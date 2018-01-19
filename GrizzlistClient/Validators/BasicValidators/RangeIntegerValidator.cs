using System.Windows;
using System.Windows.Controls;

namespace Grizzlist.Client.Validators.BasicValidators
{
    class RangeIntegerValidator : IntegerValidator
    {
        private long minimum;
        private long maximum;

        public RangeIntegerValidator(TextBox control, long minimum, long maximum)
            : base(control)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        protected override bool Validate(Control control)
        {
            if (!base.Validate(control))
                return false;

            TextBox textBox = (TextBox)control;
            long integer = long.Parse(textBox.Text);

            if (integer < minimum || integer > maximum)
            {
                textBox.Style = resource["ErrorStyleControl"] as Style;
                textBox.ToolTip = $"The number must be between {minimum} and {maximum}.";

                return false;
            }

            return true;
        }
    }
}
