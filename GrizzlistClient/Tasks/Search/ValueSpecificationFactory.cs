using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using Grizzlist.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grizzlist.Client.Tasks.Search
{
    static class ValueSpecificationFactory
    {
        public static IValueSpecification CreateStringSpecification(ValueType type, Func<Task, string> valueSelector)
        {
            TextBox textBox = new TextBox() { Margin = new Thickness(5, 7, 5, 7) };
            return new ValueSpecification<string>(type, valueSelector, textBox, x => ((TextBox)x).Text, new OperatorType[] { OperatorType.Equals, OperatorType.NotEquals, OperatorType.Contains }, new IValidator[] { new EmptyStringValidator(textBox) });
        }

        public static IValueSpecification CreateStringSpecification(ValueType type, Func<Task, string> valueSelector, OperatorType[] operators)
        {
            TextBox textBox = new TextBox() { Margin = new Thickness(5, 7, 5, 7) };
            return new ValueSpecification<string>(type, valueSelector, textBox, x => ((TextBox)x).Text, operators, new IValidator[] { new EmptyStringValidator(textBox) });
        }

        public static IValueSpecification CreateDateTimeSpecification(ValueType type, Func<Task, DateTime> valueSelector)
        {
            DatePicker datePicker = new DatePicker() { Margin = new Thickness(5), Background = new SolidColorBrush(Colors.White) };
            return new ValueSpecification<DateTime>(type, valueSelector, datePicker, x => ((DatePicker)x).SelectedDate ?? DateTime.Today, new OperatorType[] { OperatorType.Equals, OperatorType.NotEquals, OperatorType.GreaterThan, OperatorType.LessThan }, new IValidator[] { new EmptyDateTimeValidator(datePicker) });
        }

        public static IValueSpecification CreateEnumSpecification<T>(ValueType type, Func<Task, T> valueSelector)
        {
            ComboBox comboBox = new ComboBox() { Margin = new Thickness(5) };

            foreach (T value in Enum.GetValues(typeof(T)))
                comboBox.Items.Add(value);

            comboBox.SelectedItem = Enum.GetValues(typeof(T)).GetValue(0);

            return new ValueSpecification<T>(type, valueSelector, comboBox, x => (T)((ComboBox)x).SelectedItem, new OperatorType[] { OperatorType.Equals, OperatorType.NotEquals, }, new IValidator[0]);
        }

        public static IValueSpecification CreateEnumSpecification<T>(ValueType type, Func<Task, T> valueSelector, T[] values)
        {
            ComboBox comboBox = new ComboBox() { Margin = new Thickness(5) };

            foreach (T value in values)
                comboBox.Items.Add(value);

            comboBox.SelectedItem = values[0];

            return new ValueSpecification<T>(type, valueSelector, comboBox, x => (T)((ComboBox)x).SelectedItem, new OperatorType[] { OperatorType.Equals, OperatorType.NotEquals, }, new IValidator[0]);
        }
    }
}
