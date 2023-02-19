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
        public OpGroups OpGroups { get; set; } = new OpGroups("root");
        public void AddCommand(Command command, IEnumerable<string> path)
        {
            OpGroups.Add(command, path);
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

    public class Command
    {
        public string Name { get; set; }
    }
    public class OpGroups : INotifyPropertyChanged
    {
        public string OpGroupName { get; set; }
        public bool IsExpanded { get; set; } = true;
        public bool IsEmpty
        {
            get
            {
                return SubGroups.Count() == 0 && Commands.Count() == 0;
            }
        }
        public ObservableCollection<OpGroups> SubGroups { get; set; }
        public ObservableCollection<Command> Commands { get; set; }
        public OpGroups(string name)
        {
            this.OpGroupName = name;
            SubGroups = new ObservableCollection<OpGroups>();
            Commands = new ObservableCollection<Command>();
        }

        public void Add(Command command, IEnumerable<string> path)
        {
            if (command == null)
                throw new ArgumentNullException();
            if (path == null || path.Count() == 0)
                Commands.Add(command);
            if (path.Count() == 1)
            {
                this[path.First()].Commands.Add(command);
            }
            else if (path.Count() > 1)
            {
                var breadcrumb = path.First();
                var newPath = path.Skip(1);
                this[breadcrumb].Add(command, newPath);
            }
        }
        public OpGroups this[string name]
        {
            get
            {
                if (!SubGroups.Any(e => e.OpGroupName.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    SubGroups.Add(new OpGroups(name));
                }
                return SubGroups.First(e => e.OpGroupName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
        }
        public void CollapseAll(OpGroups except = null)
        {
            foreach (var item in SubGroups)
            {
                if (!CheckChild(except))
                {
                    item.IsExpanded = false;
                    item.CollapseAll();
                }
                else
                {

                }
            }
        }
        public void ExpandAll(OpGroups except = null)
        {
            foreach (var item in SubGroups)
            {
                item.IsExpanded = true;
                item.ExpandAll();
            }
        }
        public bool CheckChild(OpGroups opGroup)
        {
            if (SubGroups.Any(g => g.CheckChild(opGroup)) || SubGroups.Any(g => g.Equals(opGroup)))
            {
                return true;
            }
            return false;
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
}
