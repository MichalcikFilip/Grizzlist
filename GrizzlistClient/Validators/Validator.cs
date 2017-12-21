using System;
using System.Windows;
using System.Windows.Controls;

namespace Grizzlist.Client.Validators
{
    public abstract class Validator : IValidator
    {
        protected Control control;
        protected ResourceDictionary resource;

        public Validator(Control control)
        {
            this.control = control;
            resource = new ResourceDictionary() { Source = new Uri("/Validators/ErrorStyles.xaml", UriKind.Relative) };
        }

        public bool Validate()
        {
            if (control != null)
            {
                control.Style = null;
                control.ToolTip = null;

                return Validate(control);
            }

            return false;
        }

        protected abstract bool Validate(Control control);
    }
}
