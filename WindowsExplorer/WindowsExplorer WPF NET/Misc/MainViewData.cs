using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WindowsExplorer_WPF_NET.Misc;
using WindowsExplorer_WPF_NET.Misc.Data;
using Path = System.IO.Path;

namespace WindowsExplorer_WPF.Misc
{
    public class MainViewData : System.ComponentModel.INotifyPropertyChanged
    {
        private FileSystemWatcher _watcher;
        private Action<int, int> createRowAndColumnDefinitions;

        public ObservableCollection<FFBase> MasterViewList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
        public Dictionary<string, ObservableCollection<FFBase>> Groups { get; set; }
        public ObservableCollection<BreadCrumb> BreadCrumbs { get; set; }
        public int Rows { get; set; }
        public TreeNodeItem TreeDataRoot { get; set; } = new TreeNodeItem("", "");

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
        public MainViewData(Action<int, int> Setup)
        {
            Setup(50, 15);
            Groups = new Dictionary<string, ObservableCollection<FFBase>>() { };
            BreadCrumbs = new ObservableCollection<BreadCrumb>();
        }

        public void GetViewFromAddressString(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (_watcher != null)
                {
                    _watcher.Dispose();
                    _watcher = null;
                }
                _watcher = watch(path, new FileSystemEventHandler(fileSystemEventHandler));
                Groups.Clear();
                Groups = GetContextViewFromFileSystem(path);

                string[] breadcrumbs = path.Trim().Split(Path.DirectorySeparatorChar);
                SetBreadCrumbs(breadcrumbs);
                Task.Run(GetThumbNailsForActiveIcons);
                breadcrumbs[0] += Path.DirectorySeparatorChar;
                SetTreeViewItems(path);
            }
            else
            {
                var items = new ObservableCollection<FFBase>();
                TreeDataRoot.TreeData = new ObservableCollection<TreeNodeItem>();
                foreach (var drive in DriveInfo.GetDrives())
                {
                    items.Add(new FFBase() { Name = drive.Name, FullPath = drive.Name, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(drive.Name); }, SingleClickIcon = () => { } });

                    ShellObject shellObject = ShellObject.FromParsingName(drive.Name);
                    var bitmapSource = MainViewDataHelpers.Bitmap2BitmapImage(shellObject.Thumbnail.Bitmap);
                    TreeDataRoot.TreeData.Add(new TreeNodeItem(drive.Name, $"{drive.Name}", true, false) { Thumbnail = bitmapSource });
                }
                Groups = new Dictionary<string, ObservableCollection<FFBase>> { { "My Computer", items } };
                BreadCrumbs.Clear();
                Task.Run(GetThumbNailsForActiveIcons);
            }
        }
        public void SetTreeViewItems(string _path, bool isRegularFilePath = true, TreeNodeItem rootNode = null)
        {
            var path = _path.Split(Path.DirectorySeparatorChar);

            var searchPath = "";
            TreeNodeItem currentNode = rootNode ?? TreeDataRoot;
            var items = path.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (isRegularFilePath)
            {
                items[0] += Path.DirectorySeparatorChar;
            }

            foreach (var item in items)
            {
                currentNode = currentNode.TreeData.FirstOrDefault(x => x.Caption == item);
                if (currentNode == null)
                {
                    continue;
                }
                searchPath = Path.Combine(searchPath, item);
                if (currentNode.TreeData.Count == 0 || currentNode.TreeData.FirstOrDefault(x => x.Caption == "") != null)
                {
                    currentNode.TreeData = new ObservableCollection<TreeNodeItem>();
                    DirectoryInfo dirInfo = new DirectoryInfo(searchPath);
                    foreach (var dir in dirInfo.GetDirectories())
                    {
                        ShellObject shellObject = ShellObject.FromParsingName(dir.FullName);
                        var bitmapSource = MainViewDataHelpers.Bitmap2BitmapImage(shellObject.Thumbnail.Bitmap);
                        currentNode.TreeData.Add(new TreeNodeItem(dir.Name, dir.FullName, true, false) { Thumbnail = bitmapSource });
                    }
                }
                currentNode.IsExpanded = true;
            }

        }

        private static int SetIconArrangement(ObservableCollection<FFBase> groups, int numColumns)
        {
            int cols = numColumns;
            int col = 0;
            int row = 0;
            row++;
            foreach (var icon in groups)
            {
                if (col < cols)
                {
                    col++;
                }
                else
                {
                    col = 0;
                    row++;
                }
                icon.X = row;
                icon.Y = col;
            }

            return row;
        }

        public Dictionary<string, ObservableCollection<FFBase>> GetContextViewFromFileSystem(string path)
        {
            var di = new DirectoryInfo(path);
            var items = new ObservableCollection<FFBase>();
            foreach (var item in di.GetDirectories())
            {
                items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(item.FullName); }, SingleClickIcon = () => { } });
            }
            foreach (var item in di.GetFiles())
            {
                items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.File, DoubleClickIcon = () => { Process.Start(item.FullName); }, SingleClickIcon = () => { } });
            }
            Rows = SetIconArrangement(items, 15);
            MasterViewList = new ObservableCollection<FFBase>(items.ToList());

            return new Dictionary<string, ObservableCollection<FFBase>> { { "Main", items } };
        }

        private void fileSystemEventHandler(object sender, FileSystemEventArgs e)
        {
            GetViewFromAddressString(e.FullPath);
        }

        private FileSystemWatcher watch(string path, FileSystemEventHandler OnChanged)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes | NotifyFilters.FileName;
            watcher.Filter = "*";
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
            try
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
            catch (Exception)
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
