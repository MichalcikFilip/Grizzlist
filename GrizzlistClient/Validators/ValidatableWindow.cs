using Grizzlist.Logger;
using System.Collections.Generic;
using System.Windows;

namespace Grizzlist.Client.Validators
{
    public class ValidatableWindow : Window
    {
        private List<IValidator> validators = new List<IValidator>();

        public IEnumerable<IValidator> Validators { get { return validators; } }

        public void AddValidator(IValidator validator)
        {
            if (validator != null)
            {
                validators.Add(validator);
                Log.Debug($"Validator {validator.GetType()} was added to window", this);
            }
        }

        public void RemoveValidator(IValidator validator)
        {
            validators.Remove(validator);
        }

        public bool IsValid()
        {
            bool isValid = true;

            foreach (IValidator validator in validators)
                if (!validator.Validate()) isValid = false;

            return isValid;
        }
    }
}
