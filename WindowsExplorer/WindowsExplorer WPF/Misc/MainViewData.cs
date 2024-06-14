using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Shell;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System;

namespace WindowsExplorer_WPF.Misc
{
    public class MainViewData : System.ComponentModel.INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged = (o, e) => { };
        public Dictionary<string, ObservableCollection<FFBase>> Groups { get; set; }

        public ObservableCollection<Group> Groupings;
        //{ get { return new ObservableCollection<Group>(Groups.ToArray().Select(x => new Group(x.Key, x.Value))); } }
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
        public MainViewData()
        {
            var groupings = new ObservableCollection<Group> { };
            Groupings = groupings;
            Groups = new Dictionary<string, ObservableCollection<FFBase>>() { };
        }

        public void GetViewFromAddressString(string path)
        {
            var di = new DirectoryInfo(path);
            Groups.Clear();
            var items = new ObservableCollection<FFBase>();
            foreach (var item in di.GetDirectories())
            {
                items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(item.FullName); } });
            }
            foreach (var item in di.GetFiles())
            {
                items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.File });
            }
            Groups = new Dictionary<string, ObservableCollection<FFBase>> { { "Main", items } };

            Task.Run(GetThumbNailsForActiveIcons);
        }

        public void GetThumbNailsForActiveIcons()
        {
            foreach (var group in Groups)
            {
                foreach (var icon in group.Value)
                {
                    ShellObject shellObject = ShellObject.FromParsingName(icon.FullPath);
                    var bitmapSource = MainViewDataHelpers.Bitmap2BitmapImage(shellObject.Thumbnail.Bitmap);
                    icon.Thumbnail = bitmapSource;
                }
            }
        }
    }
    public class Group
    {
        public string Name { get; set; }
        public ObservableCollection<FFBase> Items { get; set; }
        public Group(string name, List<FFBase> items)
        {
            Name = name;
            Items = new ObservableCollection<FFBase>(items);
        }
    }
}
