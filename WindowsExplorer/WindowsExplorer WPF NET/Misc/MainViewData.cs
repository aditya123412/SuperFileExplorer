using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using WindowsExplorer_WPF_NET.Misc;
using WindowsExplorer_WPF_NET.Misc.Data;
using Path = System.IO.Path;

namespace WindowsExplorer_WPF.Misc
{
    public class MainViewData : INotifyPropertyChanged
    {
        private int rows, columns;
        //Main view variables
        public string Name { get; set; }
        public ObservableCollection<FFBase> MasterViewList { get; set; }
        public Dictionary<string, ObservableCollection<FFBase>> Groups { get; set; }
        public ObservableCollection<BreadCrumb> BreadCrumbs { get; set; }
        public ObservableCollection<GroupKeyValuePairs> SortedGroups { get; set; }
        public string MainViewAddress { get; set; }
        public GroupSortBy GroupSortBy { get; set; }
        public IEnumerable<FFBase> MainViewSelected
        {
            get
            {
                return this.Groups.SelectMany(x => x.Value.Where(y => y.Selected)).Distinct(new FFBaseEqualityComparer());
            }
        }

        // Treeview variables
        public string TreeViewAddress { get; set; }
        public TreeNodeItem TreeDataRoot { get; set; } = new TreeNodeItem("", "");

        //Misc
        public CancellationToken cancellationToken { get; set; }
        public Func<FFBase, string> GroupNamesFunc { get; set; } = (FFBase ffbase) => "Main";
        public Func<IEnumerable<FFBase>, IEnumerable<string>> SortGroupsByNameFunction { get; set; } = (IEnumerable<FFBase> fFBases) => new List<string> { "Main" };
        public Func<IEnumerable<FFBase>, IEnumerable<FFBase>> SortItemsFunction { get; set; } = (IEnumerable<FFBase> FFBases) => FFBases;
        public int Rows
        {
            get { return rows; }
            set
            {
                rows = value;
                ArrangeIcons(Rows, Columns);
            }
        }
        public int Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                ArrangeIcons(Rows, Columns);

            }
        }
        //Methods
        public MainViewData(Action<int, int> Setup)
        {
            Setup(50, 15);
            Groups = new Dictionary<string, ObservableCollection<FFBase>>() { };
            BreadCrumbs = new ObservableCollection<BreadCrumb>();
        }

        private void ArrangeIcons(int rows, int cols)
        {
            var tempList = new ObservableCollection<FFBase>();
            int row = 0, col = 0;
            foreach (var group in Groups)
            {
                col = 0;
                foreach (var fFBase in group.Value)
                {
                    fFBase.X = col;
                    fFBase.Y = row;
                    tempList.Add(fFBase);
                    if (col < cols - 1)
                    {
                        col++;
                    }
                    else
                    {
                        col = 0;
                        if (row > rows - 1)
                        {
                        }
                        row++;
                    }
                }
                row++;
            }
            MasterViewList = tempList;
        }
        public void GetViewFromAddressString(string path)
        {
            cancellationToken = new CancellationToken();
            MainViewAddress = path;
            if (!string.IsNullOrEmpty(path))
            {
                Groups.Clear();
                //Groups = GetContextViewFromFileSystem(path);
                GetContextViewFromFileSystem(path);

                SetBreadCrumbs(path);
                Task.Run(GetThumbNailsForActiveIcons, cancellationToken);
                SetTreeViewItems(path);
            }
            else
            {
                ShowHomeInMainView();
            }
            ArrangeIcons(Rows, Columns);
        }
        public void Refresh()
        {
            var di = new DirectoryInfo(MainViewAddress);
            var items = new ObservableCollection<FFBase>();
            foreach (var item in di.GetDirectories())
            {
                items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(item.FullName); }, SingleClickIcon = () => { } });
            }
            foreach (var item in di.GetFiles())
            {
                items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.File, DoubleClickIcon = () => { Process.Start(item.FullName); }, SingleClickIcon = () => { } });
            }
            var addedItems = items.Where(item => !MasterViewList.Any(mvlItem => mvlItem.FullPath.Equals(item.FullPath, StringComparison.CurrentCultureIgnoreCase)));
            foreach (var item in items)
            {
                var match = MasterViewList.FirstOrDefault(mvlItem => mvlItem.FullPath.Equals(item.FullPath, StringComparison.CurrentCultureIgnoreCase));
                if (match == null)
                {
                    ShellObject shellObject = ShellObject.FromParsingName(item.FullPath);
                    var bitmapSource = MainViewDataHelpers.Bitmap2BitmapImage(shellObject.Thumbnail.Bitmap);
                    item.Thumbnail = bitmapSource;
                }
                else
                {
                    item.Thumbnail = match.Thumbnail;
                }
            }
            MasterViewList = new ObservableCollection<FFBase>(items.ToList());
            Groups = new Dictionary<string, ObservableCollection<FFBase>> { { "Main", items } };
        }
        private void ShowHomeInMainView()
        {
            var items = new ObservableCollection<FFBase>();
            TreeDataRoot.TreeData = new ObservableCollection<TreeNodeItem>();
            int column = 0;
            foreach (var drive in DriveInfo.GetDrives())
            {
                items.Add(new FFBase() { Name = drive.Name, X = column++, FullPath = drive.Name, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(drive.Name); }, SingleClickIcon = () => { } });

                ShellObject shellObject = ShellObject.FromParsingName(drive.Name);
                var bitmapSource = MainViewDataHelpers.Bitmap2BitmapImage(shellObject.Thumbnail.Bitmap);
                TreeDataRoot.TreeData.Add(new TreeNodeItem(drive.Name, $"{drive.Name}", true, false) { Thumbnail = bitmapSource });
            }
            MasterViewList = new ObservableCollection<FFBase>(items.ToList());
            Groups = new Dictionary<string, ObservableCollection<FFBase>> { { "My Computer", items } };

            Task.Run(GetThumbNailsForActiveIcons);
            GetGroupBy(GroupNamesFunc, SortGroupsByNameFunction, SortItemsFunction);
            BreadCrumbs.Clear();
        }
        public Dictionary<string, ObservableCollection<FFBase>> GetContextViewFromFileSystem(string path)
        {
            var di = new DirectoryInfo(path);
            var items = new ObservableCollection<FFBase>();
            int column = 0;
            foreach (var item in di.GetDirectories())
            {
                items.Add(new FFBase() { Name = item.Name, X = column++, FullPath = item.FullName, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(item.FullName); }, SingleClickIcon = () => { } });
            }
            foreach (var item in di.GetFiles())
            {
                items.Add(new FFBase() { Name = item.Name, X = column++, FullPath = item.FullName, Type = Type.File, DoubleClickIcon = () => { Process.Start(item.FullName); }, SingleClickIcon = () => { } });
            }
            MasterViewList = new ObservableCollection<FFBase>(items.ToList());
            GetGroupBy(GroupNamesFunc, SortGroupsByNameFunction, SortItemsFunction);
            return new Dictionary<string, ObservableCollection<FFBase>> { { "Main", items } };
        }

        public void SetTreeViewItems(string _path, bool isRegularFilePath = true, TreeNodeItem rootNode = null)
        {
            TreeViewAddress = _path;
            var items = _path.Split(Path.DirectorySeparatorChar).Where(x => !string.IsNullOrEmpty(x)).ToArray()
                .Select(x => x.Replace(":", $":{Path.DirectorySeparatorChar}"));

            var searchPath = "";
            TreeNodeItem currentNode = rootNode == null ? TreeDataRoot : rootNode;

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

        private void fileSystemEventHandler(object sender, FileSystemEventArgs e)
        {
            GetViewFromAddressString(e.FullPath);
        }

        private FileSystemWatcher watch(string path, FileSystemEventHandler OnChanged)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            //watcher.Path = path;
            //watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes | NotifyFilters.FileName;
            //watcher.Filter = "*";
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.EnableRaisingEvents = true;
            return watcher;
        }

        // Sort and group by
        public void SortWithinGroup(FieldName sortByField)
        {
            foreach (var group in Groups)
            {
                group.Value.OrderBy(f => f[sortByField]);
            }
        }

        public void GroupBy(Func<FFBase, string> groupNameFunc,
            Func<IEnumerable<FFBase>, IEnumerable<string>> sortGroupsByNameFunction,
            Func<IEnumerable<FFBase>, IEnumerable<FFBase>> sortItemsFunction)
        {
            this.GroupNamesFunc = groupNameFunc;
            this.SortGroupsByNameFunction = sortGroupsByNameFunction;
            this.SortItemsFunction = sortItemsFunction;
            GetGroupBy(groupNameFunc, sortGroupsByNameFunction, sortItemsFunction);
        }
        public void GetGroupBy(Func<FFBase, string> groupNameFunc,
            Func<IEnumerable<FFBase>, IEnumerable<string>> sortGroupsByNameFunction,
            Func<IEnumerable<FFBase>, IEnumerable<FFBase>> sortItemsFunction)
        {
            var groupNames = sortGroupsByNameFunction(MasterViewList);

            var tempGroups = new Dictionary<string, ObservableCollection<FFBase>>();
            foreach (var groupName in groupNames)
            {
                tempGroups.Add(groupName, new ObservableCollection<FFBase>(MasterViewList.Where(x => groupNameFunc(x).Equals(groupName))));
            }
            Groups = tempGroups;
            ArrangeIcons(Rows, Columns);
        }
        public void SortGroupNames(Dictionary<string, ObservableCollection<FFBase>> groups, GroupSortBy groupSortBy, bool Desc = false)
        {
            var groupticles = groups.Keys.Select(v => (GroupName: v, GroupItems: groups[v]));
            groups.Clear();
            switch (groupSortBy)
            {
                case GroupSortBy.Name:
                    groupticles = groupticles.OrderBy(x => x.GroupName);
                    groupticles.ToList()
                    .ForEach((x) => { groups.Add(x.GroupName, x.GroupItems); });
                    break;
                case GroupSortBy.Count:
                    groupticles = groupticles.OrderBy(g => g.GroupItems.Count());
                    groupticles.ToList()
                        .ForEach((x) => { groups.Add(x.GroupName, x.GroupItems); });
                    break;
                case GroupSortBy.NewestItem:
                    groupticles = groupticles.OrderBy(g => g.GroupItems.Max(n => n.Created));
                    groupticles.ToList()
                        .ForEach((x) => { groups.Add(x.GroupName, x.GroupItems); });
                    break;
                case GroupSortBy.OldestItem:
                    groupticles = groupticles.OrderBy(g => g.GroupItems.Min(n => n.Created));
                    groupticles.ToList()
                        .ForEach((x) => { groups.Add(x.GroupName, x.GroupItems); });
                    break;
                case GroupSortBy.LastModified:
                    groupticles = groupticles.OrderBy(g => g.GroupItems.Max(n => n.LastModified));
                    groupticles.ToList()
                        .ForEach((x) => { groups.Add(x.GroupName, x.GroupItems); });
                    break;
                case GroupSortBy.AsIs:
                    break;
                default:
                    break;
            }
            Groups = groups;
        }

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
        public void SetBreadCrumbs(string _path)
        {
            var breadCrumbs = _path.Trim().Split(Path.DirectorySeparatorChar).Where(x => x.Length > 0).Select(x => x.Replace(":", $":{Path.DirectorySeparatorChar}"));
            var path = "";
            BreadCrumbs.Clear();

            foreach (var breadCrumb in breadCrumbs)
            {
                path = Path.Combine(path, breadCrumb);
                BreadCrumbs.Add(new BreadCrumb(breadCrumb, path));
                BreadCrumbs.Add(new BreadCrumb($"{Path.DirectorySeparatorChar}", path));
            }
        }

        public void Copy() { }
        public void Paste() { }
        public void Rename() { }
        public void Delete() { }
        public void Tag() { }
        public void BookMark() { }

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
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
    }

    public enum GroupSortBy
    {
        Name, Count, NewestItem, OldestItem, LastModified, TotalSize, AsIs
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
    public class ViewData : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ObservableCollection<FFBase> MasterViewList { get; set; }
        public Dictionary<string, ObservableCollection<FFBase>> Groups { get; set; }
        public ObservableCollection<BreadCrumb> BreadCrumbs { get; set; }
        public string TreeViewAddress { get; set; }
        public string MainViewAddress { get; set; }
        public IEnumerable<FFBase> MainViewSelected { get; set; }
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
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
    }

    public class GroupKeyValuePairs : INotifyPropertyChanged
    {
        public string GroupName;
        public List<FFBase> GroupItems;

        public GroupKeyValuePairs(string groupName, List<FFBase> value)
        {
            GroupName = groupName;
            GroupItems = value;
        }

        public override bool Equals(object obj)
        {
            return obj is GroupKeyValuePairs other &&
                   GroupName == other.GroupName &&
                   EqualityComparer<List<FFBase>>.Default.Equals(GroupItems, other.GroupItems);
        }

        public override int GetHashCode()
        {
            int hashCode = -1312193335;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GroupName);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<FFBase>>.Default.GetHashCode(GroupItems);
            return hashCode;
        }

        public void Deconstruct(out string groupName, out List<FFBase> value)
        {
            groupName = GroupName;
            value = GroupItems;
        }

        public static implicit operator (string GroupName, List<FFBase> Value)(GroupKeyValuePairs value)
        {
            return (value.GroupName, value.GroupItems);
        }

        public static implicit operator GroupKeyValuePairs((string GroupName, List<FFBase> Value) value)
        {
            return new GroupKeyValuePairs(value.GroupName, value.Value);
        }
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
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
    }
}
