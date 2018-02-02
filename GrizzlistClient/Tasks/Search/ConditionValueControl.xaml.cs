using Grizzlist.Client.Validators;
using Grizzlist.Tasks;
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
            specifications.Add(ValueType.Description, ValueSpecificationFactory.CreateStringSpecification(ValueType.Description, x => x.Description));
            specifications.Add(ValueType.Note, ValueSpecificationFactory.CreateStringSpecification(ValueType.Note, x => x.Note));
            specifications.Add(ValueType.Created, ValueSpecificationFactory.CreateDateTimeSpecification(ValueType.Created, x => x.Created));
            specifications.Add(ValueType.Closed, ValueSpecificationFactory.CreateDateTimeSpecification(ValueType.Closed, x => x.Closed));
            specifications.Add(ValueType.Priority, ValueSpecificationFactory.CreateEnumSpecification(ValueType.Priority, x => x.Priority));
            specifications.Add(ValueType.State, ValueSpecificationFactory.CreateEnumSpecification(ValueType.State, x => x.State, new TaskState[] { TaskState.Open, TaskState.Postponed, TaskState.Closed, TaskState.Archived }));
            specifications.Add(ValueType.Deadline, ValueSpecificationFactory.CreateDateTimeSpecification(ValueType.Deadline, x => x.Deadline));
            specifications.Add(ValueType.Tags, ValueSpecificationFactory.CreateStringSpecification(ValueType.Tags, x => string.Join(",", x.Tags.Select(y => y.Value)), new OperatorType[] { OperatorType.Contains }));
            specifications.Add(ValueType.SubtasksNames, ValueSpecificationFactory.CreateStringSpecification(ValueType.SubtasksNames, x => string.Join(",", x.SubTasks.Select(y => y.Name)), new OperatorType[] { OperatorType.Contains }));
            specifications.Add(ValueType.SubtasksDescriptions, ValueSpecificationFactory.CreateStringSpecification(ValueType.SubtasksDescriptions, x => string.Join(",", x.SubTasks.Select(y => y.Description)), new OperatorType[] { OperatorType.Contains }));

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
