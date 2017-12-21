using Grizzlist.Client.Validators;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Grizzlist.Client.Tasks.Validators
{
    class DeadlineValidator : Validator
    {
        private DateTime minimum;

        public DeadlineValidator(DatePicker control, DateTime minimum)
            : base(control)
        {
            this.minimum = minimum;
        }

        protected override bool Validate(Control control)
        {
            DatePicker datePicker = (DatePicker)control;

            if (datePicker.SelectedDate.GetValueOrDefault() < minimum)
            {
                datePicker.Style = resource["ErrorStyleControl"] as Style;
                datePicker.ToolTip = "Deadline can't be less than the current date.";

                return false;
            }

            return true;
        }
    }
}
