using Grizzlist.Client.Validators;
using Grizzlist.Tasks.Templates;
using System;
using System.Windows.Input;

namespace Grizzlist.Client.Tasks.Templates
{
    /// <summary>
    /// Interaction logic for ConditionOperatorControl.xaml
    /// </summary>
    public partial class ConditionOperatorControl : ValidatableControl, IConditionControl
    {
        public event Action RemoveClicked;

        public ConditionOperatorControl(ConditionOperator condition)
        {
            InitializeComponent();

            foreach (ConditionOperatorType type in Enum.GetValues(typeof(ConditionOperatorType)))
                cbType.Items.Insert(0, type);

            cbType.SelectedItem = ConditionOperatorType.And;

            if (condition != null)
                cbType.SelectedItem = condition.Type;
        }

        public override bool IsValid()
        {
            return base.IsValid();
        }

        public ICondition GetCondition()
        {
            if (IsValid())
                return new ConditionOperator() { Type = (ConditionOperatorType)cbType.SelectedItem };

            return null;
        }

        private void Remove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RemoveClicked?.Invoke();
        }
    }
}
