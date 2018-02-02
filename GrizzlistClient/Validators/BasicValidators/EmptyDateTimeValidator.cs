using System.Windows;
using System.Windows.Controls;

namespace Grizzlist.Client.Validators.BasicValidators
{
    class EmptyDateTimeValidator : Validator
    {
        public EmptyDateTimeValidator(DatePicker control)
            : base(control)
        { }

        protected override bool Validate(Control control)
        {
            DatePicker datePicker = (DatePicker)control;

            if (datePicker.SelectedDate == null)
            {
                datePicker.Style = resource["ErrorStyleControl"] as Style;
                datePicker.ToolTip = "This field cannot be empty.";

                return false;
            }

            return true;
        }
    }
}
