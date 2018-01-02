using Grizzlist.Client.BackgroundActions;
using Grizzlist.Client.Persistent;
using Grizzlist.Client.UserSettings;
using Grizzlist.Extensions;
using Grizzlist.Logger;
using Grizzlist.Persistent;
using Grizzlist.Tasks;
using Grizzlist.Tasks.Templates;
using System;

namespace Grizzlist.Client.Tasks.Templates.BackgroundActions
{
    class TemplatesAction : BaseAction
    {
        public event Action<Task> TaskCreated;

        public TemplatesAction()
            : base(3600)
        {
            DateTime lastRun = DateTime.Today, today = DateTime.Today;

            using (IRepository<Settings, long> repository = PersistentFactory.GetContext().GetRepository<Settings, long>())
            {
                Settings settings = SettingsFactory.GetSettings(repository);
                lastRun = settings.LastRun.GetDate();
            }

            if (lastRun < today)
            {
                using (IRepository<Template, long> repository = PersistentFactory.GetContext().GetRepository<Template, long>())
                {
                    while (lastRun < today)
                    {
                        foreach (Template template in repository.GetAll())
                        {
                            if (template.SatisfiesCondition(lastRun))
                            {
                                Task newTask = template.CreateTask(lastRun);
                                TaskCreated?.Invoke(newTask);

                                repository.Update(template);
                                Log.Info($"Task {newTask.Name} was created from template for day {lastRun.ToShortDateString()}", this);
                            }
                        }

                        lastRun = lastRun.AddDays(1);
                    }
                }
            }
        }

        protected override void RunAction()
        {
            using (IRepository<Template, long> repository = PersistentFactory.GetContext().GetRepository<Template, long>())
            {
                foreach (Template template in repository.GetAll())
                {
                    if (template.SatisfiesCondition())
                    {
                        Task newTask = template.CreateTask();
                        TaskCreated?.Invoke(newTask);

                        repository.Update(template);
                        Log.Info($"Task {newTask.Name} was created from template", this);
                    }
                }
            }
        }
    }
}
