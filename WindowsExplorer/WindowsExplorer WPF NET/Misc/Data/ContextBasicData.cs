using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using WindowsExplorer_WPF;
using WindowsExplorer_WPF.Misc;
using WindowsExplorer_WPF.Misc.Helpers;
using Type = WindowsExplorer_WPF.Misc.Type;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    public class ContextBasicData : INotifyPropertyChanged
    {
        private int rows, columns;
        public string Header { get; set; }
        public ObservableCollection<FFBase> MasterViewList { get; set; } = new ObservableCollection<FFBase>();
        public Dictionary<string, ObservableCollection<FFBase>> Groups { get; set; }
        public Grid MainGrid { get; internal set; }
        public string TreeViewAddress { get; set; }
        public TreeNodeItem TreeDataRoot { get; set; } = new TreeNodeItem("", "");
        public ObservableCollection<BreadCrumb> BreadCrumbs { get; set; } = new ObservableCollection<BreadCrumb>();
        public Func<FFBase, string> GroupNamesFunc { get; set; } = (FFBase ffbase) => "Main";
        public Func<IEnumerable<FFBase>, IEnumerable<string>> SortGroupsByNameFunction { get; set; } = (IEnumerable<FFBase> fFBases) => new List<string> { "Main" };
        public Func<IEnumerable<FFBase>, IEnumerable<FFBase>> SortItemsFunction { get; set; } = (IEnumerable<FFBase> FFBases) => FFBases;
        public int Rows
        {
            get { return rows; }
            set
            {
                rows = value;
                ResizeGrid(rows, Columns);
                ArrangeIcons(Rows, Columns);
            }
        }
        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                ResizeGrid(Rows, columns);
                ArrangeIcons(Rows, Columns);
            }
        }
        public IEnumerable<FFBase> MainViewSelected
        {
            get
            {
                return this.Groups.SelectMany(x => x.Value.Where(y => y.Selected)).Distinct(new FFBaseEqualityComparer());
            }
        }
        public string MainViewAddress { get; private set; }
        public CancellationToken cancellationToken { get; private set; }
        public string Name { get; private set; }

        public void ArrangeIcons(int rows, int cols)
        {
            var tempList = new ObservableCollection<FFBase>();
            int row = 0, col;
            MasterViewList.Clear();
            if (Groups != null && MasterViewList != null)
            {
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
                                rows++;
                            }
                            row++;
                        }
                    }
                    row++;
                }
                ResizeGrid(row, Columns);
                MasterViewList = tempList;
            }
        }
        public void ResizeGrid(int rows, int cols)
        {
            if (MainGrid == null)
                return;
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();
            this.rows = rows;
            this.columns = cols;
            for (int i = 0; i < rows; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < cols; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            MainGrid.UpdateLayout();
            MainGrid.InvalidateArrange();
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
                TreeDataRoot.TreeData.Add(new TreeNodeItem(drive.Name, $"{drive.Name}", true, false) { Thumbnail = bitmapSource, Context = this });
            }
            MasterViewList = new ObservableCollection<FFBase>(items.ToList());
            Groups = new Dictionary<string, ObservableCollection<FFBase>> { { "My Computer", items } };

            Task.Run(() => { GetThumbNailsForActiveIcons(); });
            GetGroupBy(GroupNamesFunc, SortGroupsByNameFunction, SortItemsFunction);
            Name = "Home";
            BreadCrumbs.Clear();
        }
        public Dictionary<string, ObservableCollection<FFBase>> GetContextViewFromFileSystem(string path)
        {
            var di = new DirectoryInfo(path);
            MasterViewList = new ObservableCollection<FFBase>();
            int column = 0;
            foreach (var item in di.GetDirectories())
            {
                MasterViewList.Add(new FFBase() { Name = item.Name, X = column++, FullPath = item.FullName, Type = Type.Folder, DoubleClickIcon = () => { GetViewFromAddressString(item.FullName); }, SingleClickIcon = () => { } });
            }
            foreach (var item in di.GetFiles())
            {
                MasterViewList.Add(new FFBase() { Name = item.Name, X = column++, FullPath = item.FullName, Type = Type.File, DoubleClickIcon = () => { Process.Start(item.FullName); }, SingleClickIcon = () => { } });
            }
            //MasterViewList = new ObservableCollection<FFBase>(items.ToList());
            GetGroupBy(GroupNamesFunc, SortGroupsByNameFunction, SortItemsFunction);
            Name = di.Name;
            return new Dictionary<string, ObservableCollection<FFBase>> { { "Main", MasterViewList } };
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
                Task.Run(() => { GetThumbNailsForActiveIcons(); }, cancellationToken);
                SetTreeViewItems(path);
                Name = Path.GetDirectoryName(path);
            }
            else
            {
                ShowHomeInMainView();
                Name = "Home";
            }
            ArrangeIcons(Rows, Columns);
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
        public void SetBreadCrumbs(string _path)
        {
            var breadCrumbs = _path.Trim().Split(Path.DirectorySeparatorChar).Where(x => x.Length > 0).Select(x => x.Replace(":", $":{Path.DirectorySeparatorChar}"));
            var path = "";
            BreadCrumbs.Clear();

            foreach (var breadCrumb in breadCrumbs)
            {
                path = Path.Combine(path, breadCrumb);
                BreadCrumbs.Add(new BreadCrumb(breadCrumb, path, this));
                BreadCrumbs.Add(new BreadCrumb($"{Path.DirectorySeparatorChar}", path, this));
            }
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
                        currentNode.TreeData.Add(new TreeNodeItem(dir.Name, dir.FullName, true, false) { Thumbnail = bitmapSource, Context = this });
                    }
                }
                currentNode.IsExpanded = true;
            }
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
