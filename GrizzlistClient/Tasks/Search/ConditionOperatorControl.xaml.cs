using Grizzlist.Client.Validators;
using Grizzlist.Tasks.Templates;
using System;
using System.Windows;
using System.Windows.Input;

namespace Grizzlist.Client.Tasks.Search
{
    /// <summary>
    /// Interaction logic for ConditionOperatorControl.xaml
    /// </summary>
    public partial class ConditionOperatorControl : ValidatableControl, IConditionControl
    {
        public event Action RemoveClicked;

        public ConditionOperatorControl()
        {
            InitializeComponent();

            btnConOperator.MouseLeftButtonDown += (s, e) => AddCondition(new ConditionOperatorControl());
            btnConValue.MouseLeftButtonDown += (s, e) => AddCondition(new ConditionValueControl());

            foreach (ConditionOperatorType type in Enum.GetValues(typeof(ConditionOperatorType)))
                cbType.Items.Add(type);

            cbType.SelectedItem = ConditionOperatorType.And;
        }

        private void AddCondition(ValidatableControl conditionControl)
        {
            conditionControl.Margin = new Thickness(2, 2, 0, 2);

            if (conditionControl is IConditionControl)
                ((IConditionControl)conditionControl).RemoveClicked += () => pnlCondition.Children.Remove(conditionControl);

            pnlCondition.Children.Add(conditionControl);
        }

        public override bool IsValid()
        {
            foreach (ValidatableControl control in pnlCondition.Children)
                if (!control.IsValid())
                    return false;

            return true;
        }

        public ICondition GetCondition()
        {
            if (IsValid())
            {
                ConditionOperator condition = new ConditionOperator() { Type = (ConditionOperatorType)cbType.SelectedItem };

                foreach (IConditionControl control in pnlCondition.Children)
                    condition.Conditions.Add(control.GetCondition());

                return condition;
            }

            return null;
        }

        private void Remove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RemoveClicked?.Invoke();
        }
    }
}
