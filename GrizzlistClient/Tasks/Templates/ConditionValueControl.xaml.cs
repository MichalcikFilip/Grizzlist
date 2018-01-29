using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using Grizzlist.Tasks.Templates;
using System;
using System.Windows.Input;

namespace Grizzlist.Client.Tasks.Templates
{
    /// <summary>
    /// Interaction logic for ConditionValueControl.xaml
    /// </summary>
    public partial class ConditionValueControl : ValidatableControl, IConditionControl
    {
        public event Action RemoveClicked;

        public ConditionValueControl(ConditionValue condition = null)
        {
            InitializeComponent();

            foreach (ConditionValueType type in Enum.GetValues(typeof(ConditionValueType)))
                cbType.Items.Insert(0, type);

            cbType.SelectedItem = ConditionValueType.DayInMonth;
            tbValue.Text = "0";

            AddValidator(new RangeIntegerValidator(tbValue, 0, 100));

            if (condition != null)
            {
                cbType.SelectedItem = condition.Type;
                tbValue.Text = condition.Value.ToString();
            }
        }

        public ICondition GetCondition()
        {
            if (IsValid())
                return new ConditionValue() { Type = (ConditionValueType)cbType.SelectedItem, Value = short.Parse(tbValue.Text) };

            return null;
        }

        private void Remove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RemoveClicked?.Invoke();
        }
    }
}
