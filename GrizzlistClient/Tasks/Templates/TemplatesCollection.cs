using Grizzlist.Client.Collection;
using Grizzlist.Client.Persistent;
using Grizzlist.Persistent;
using Grizzlist.Tasks.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Grizzlist.Client.Tasks.Templates
{
    class TemplatesCollection : ICollectionManager
    {
        public IEnumerable<CollectionItem> LoadItems()
        {
            using (IRepository<Template, long> repository = PersistentFactory.GetContext().GetRepository<Template, long>())
                return repository.GetAll().Select(x => new CollectionItem(new TemplateItem(x)));
        }

        public CollectionItem AddItem(Window owner)
        {
            EditTemplateWindow window = new EditTemplateWindow(owner);

            if (window.ShowDialog() ?? false)
            {
                using (IRepository<Template, long> repository = PersistentFactory.GetContext().GetRepository<Template, long>())
                    repository.Add(window.EditedTemplate);

                return new CollectionItem(new TemplateItem(window.EditedTemplate));
            }

            return null;
        }

        public void UpdateItem(CollectionItem item, Window owner)
        {
            EditTemplateWindow window = new EditTemplateWindow(owner, ((TemplateItem)item.ItemContent).TaskTemplate);

            if (window.ShowDialog() ?? false)
            {
                using (IRepository<Template, long> repository = PersistentFactory.GetContext().GetRepository<Template, long>())
                    repository.Update(window.EditedTemplate);

                ((TemplateItem)item.ItemContent).Update();
            }
        }

        public bool RemoveItem(CollectionItem item)
        {
            using (IRepository<Template, long> repository = PersistentFactory.GetContext().GetRepository<Template, long>())
                repository.Remove(((TemplateItem)item.ItemContent).TaskTemplate);

            return true;
        }

        public bool MatchSearch(CollectionItem item, string text)
        {
            text = text.Trim().ToLower();
            Template template = ((TemplateItem)item.ItemContent).TaskTemplate;
            return template.Task.Name.ToLower().Contains(text) || template.Task.Description.ToLower().Contains(text);
        }
    }
}
