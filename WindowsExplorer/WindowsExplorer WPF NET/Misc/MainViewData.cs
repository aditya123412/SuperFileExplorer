using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Shell;
using WindowsExplorer_WPF_NET.Misc.Data;
using Path = System.IO.Path;

namespace WindowsExplorer_WPF.Misc
{
    public class MainViewData : System.ComponentModel.INotifyPropertyChanged
    {
        private FileSystemWatcher _watcher;
        public ObservableCollection<FFBase> MasterViewList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
        public Dictionary<string, ObservableCollection<FFBase>> Groups { get; set; }
        public ObservableCollection<BreadCrumb> BreadCrumbs { get; set; }
        public ObservableCollection<WindowsExplorer_WPF_NET.Misc.TreeNodeItem> TreeData { get; set; }

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
            Groups = new Dictionary<string, ObservableCollection<FFBase>>() { };
            BreadCrumbs = new ObservableCollection<BreadCrumb>();
        }

        public void GetViewFromAddressString(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var di = new DirectoryInfo(path);
                if (_watcher!=null)
                {
                    _watcher.Dispose();
                    _watcher = null;
                }
                _watcher = watch(path,new FileSystemEventHandler(fileSystemEventHandler));
                Groups.Clear();
                var items = new ObservableCollection<FFBase>();
                foreach (var item in di.GetDirectories())
                {
                    items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(item.FullName); } });
                }
                foreach (var item in di.GetFiles())
                {
                    items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.File, DoubleClickIcon = () => { Process.Start(item.FullName); } });
                }
                MasterViewList = new ObservableCollection<FFBase>(items.ToList());
                Groups = new Dictionary<string, ObservableCollection<FFBase>> { { "Main", items } };
                SetBreadCrumbs(path.Trim().Split(System.IO.Path.DirectorySeparatorChar));
                Task.Run(GetThumbNailsForActiveIcons);
                SetTreeViewItems(path.Trim().Split(System.IO.Path.DirectorySeparatorChar));
            }
            else
            {
                var items = new ObservableCollection<FFBase>();
                TreeData = new ObservableCollection<WindowsExplorer_WPF_NET.Misc.TreeNodeItem>();
                foreach (var drive in DriveInfo.GetDrives())
                {
                    items.Add(new FFBase() { Name = drive.Name, FullPath = drive.Name, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(drive.Name); } });
                    TreeData.Add(new WindowsExplorer_WPF_NET.Misc.TreeNodeItem(drive.Name, drive.Name));
                }
                Groups = new Dictionary<string, ObservableCollection<FFBase>> { { "My Computer", items } };
                BreadCrumbs.Clear();
                Task.Run(GetThumbNailsForActiveIcons);
            }
            TreeData[0].TreeData = TreeData;
        }

        private void fileSystemEventHandler(object sender, FileSystemEventArgs e)
        {
            GetViewFromAddressString(e.FullPath);
        }

        private FileSystemWatcher watch(string path, FileSystemEventHandler OnChanged)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
            return watcher;
        }
        public void Sort(FieldName sortByField)
        {
            foreach (var group in Groups)
            {
                group.Value.Sort(Comparer<FFBase>.Create((f1, f2) => (int)f1[sortByField] - (int)f2[sortByField]));
            }
        }

        public void GroupBy(FieldName groupByField)
        {
            Groups = MasterViewList.GroupBy((item) => item.Type.ToString());
        }

        public void Copy() { }
        public void Paste() { }
        public void Rename() { }
        public void Delete() { }
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
        public void SetTreeViewItems(string[] path)
        {
            var searchPath = "";
            foreach (var item in path)
            {

            }
        }
        public void SetBreadCrumbs(string[] breadCrumbs)
        {
            var path = "";
            BreadCrumbs.Clear();

            foreach (var breadCrumb in breadCrumbs)
            {
                //path = string.Join(Path.DirectorySeparatorChar.ToString(), new string[] { path, breadCrumb });
                path = Path.Combine(path, breadCrumb);
                BreadCrumbs.Add(new BreadCrumb(breadCrumb, path));
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
    public class BreadCrumb
    {
        public string Caption { get; set; }
        public string FullPath { get; set; }

        public BreadCrumb(string caption, string fullPath)
        {
            Caption = caption;
            FullPath = fullPath;
        }
    }
}
