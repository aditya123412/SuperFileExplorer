using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    public class CommandsMenuContext
    {
        public ObservableCollection<CommandsCategory> CommandsGroups { get; set; } = new ObservableCollection<CommandsCategory>();

        public void AddCommand(Command command, string Category, string GroupName)
        {
            if (!CommandsGroups.Any(category => category.CategoryName.Equals(Category)))
            {
                CommandsGroups.Add(new CommandsCategory(Category));
            }

        }
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

    }

    public class CommandsCategory
    {
        public string CategoryName { get; set; }
        public ObservableCollection<CommandsGroup> Commands { get; set; } = new ObservableCollection<CommandsGroup>();

        public CommandsCategory(string name)
        {
            CategoryName = name;
        }

    }

    public class CommandsGroup
    {
        public string CommandsGroupName;
        public ObservableCollection<Command> CommandsCollection { get; set; }
        public Command this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                return CommandsCollection.First(x => x.Name.Equals(name));
            }
        }
    }

    public class Command
    {
        public string Name { get; set; }
    }
}
