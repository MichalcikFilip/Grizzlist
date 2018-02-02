using Grizzlist.Client.Validators;
using System.Windows;

namespace Grizzlist.Client.Tasks.Search
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow(Window owner)
        {
            Owner = owner;

            InitializeComponent();

            btnConOperator.MouseLeftButtonDown += (s, e) => AddCondition(new ConditionOperatorControl());
            btnConValue.MouseLeftButtonDown += (s, e) => AddCondition(new ConditionValueControl());
        }

        private void AddCondition(ValidatableControl conditionControl)
        {
            btnConOperator.Visibility = Visibility.Collapsed;
            btnConValue.Visibility = Visibility.Collapsed;
            conditionControl.Margin = new Thickness(4);

            if (conditionControl is IConditionControl)
            {
                ((IConditionControl)conditionControl).RemoveClicked += () =>
                {
                    btnConOperator.Visibility = Visibility.Visible;
                    btnConValue.Visibility = Visibility.Visible;

                    pnlCondition.Children.Clear();
                };
            }

            pnlCondition.Children.Add(conditionControl);
        }
    }
}
