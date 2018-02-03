using Grizzlist.Client.Persistent;
using Grizzlist.Client.Validators;
using Grizzlist.Persistent;
using Grizzlist.Tasks;
using System.Linq;
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

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (pnlCondition.Children.Count == 1 && pnlCondition.Children[0] is ValidatableControl && ((ValidatableControl)pnlCondition.Children[0]).IsValid() && pnlCondition.Children[0] is IConditionControl)
            {
                ICondition condition = ((IConditionControl)pnlCondition.Children[0]).GetCondition();
                pnlTasks.Children.Clear();

                using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                {
                    foreach (Task task in repository.GetAll().Where(x => x.State != TaskState.Removed))
                    {
                        if (condition.Satisfies(task))
                        {
                            SearchItemControl control = new SearchItemControl(this, task);
                            control.OnSelect += t => Deselect();
                            pnlTasks.Children.Add(control);
                        }
                    }

                    tbResult.Text = $"{pnlTasks.Children.Count} tasks was found";
                }
            }
        }

        private void Deselect()
        {
            foreach (SearchItemControl control in pnlTasks.Children)
                control.Deselect();
        }
    }
}
