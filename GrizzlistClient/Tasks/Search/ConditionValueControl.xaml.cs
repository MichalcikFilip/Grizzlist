using Grizzlist.Client.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Grizzlist.Client.Tasks.Search
{
    /// <summary>
    /// Interaction logic for ConditionValueControl.xaml
    /// </summary>
    public partial class ConditionValueControl : ValidatableControl, IConditionControl
    {
        private Dictionary<ValueType, IValueSpecification> specifications = new Dictionary<ValueType, IValueSpecification>();

        public event Action RemoveClicked;

        public ConditionValueControl()
        {
            specifications.Add(ValueType.Name, ValueSpecificationFactory.CreateStringSpecification(ValueType.Name, x => x.Name));
            specifications.Add(ValueType.Created, ValueSpecificationFactory.CreateDateTimeSpecification(ValueType.Created, x => x.Created));

            InitializeComponent();

            foreach (ValueType type in Enum.GetValues(typeof(ValueType)))
                cbType.Items.Add(type);

            cbType.SelectedItem = ValueType.Name;
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbOperator.Items.Clear();

            foreach (OperatorType type in specifications[(ValueType)cbType.SelectedItem].Operators)
                cbOperator.Items.Add(type);

            cbOperator.SelectedItem = specifications[(ValueType)cbType.SelectedItem].Operators.First();

            ClearValidators();

            foreach (IValidator validator in specifications[(ValueType)cbType.SelectedItem].Validators)
                AddValidator(validator);

            if (e.RemovedItems.Count > 0)
                mainGrid.Children.Remove(specifications[(ValueType)e.RemovedItems[0]].Control);

            mainGrid.Children.Add(specifications[(ValueType)cbType.SelectedItem].Control);
            Grid.SetColumn(specifications[(ValueType)cbType.SelectedItem].Control, 2);
        }

        public ICondition GetCondition()
        {
            if (IsValid())
                return specifications[(ValueType)cbType.SelectedItem].CreateCondition((OperatorType)cbOperator.SelectedItem);

            return null;
        }

        private void Remove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RemoveClicked?.Invoke();
        }
    }
}
