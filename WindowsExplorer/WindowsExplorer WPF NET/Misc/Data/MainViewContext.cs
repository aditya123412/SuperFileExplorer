using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WindowsExplorer_WPF.Misc.Helpers;
using WindowsExplorer_WPF_NET.Controls;
using WindowsExplorer_WPF_NET.Misc;
using WindowsExplorer_WPF_NET.Misc.Data;
using Path = System.IO.Path;

namespace WindowsExplorer_WPF.Misc
{
    public class MainViewContext : INotifyPropertyChanged
    {
        public static MainViewContext CommonInstance = null;
        private static Dictionary<string, MainViewContext> Contexts = new Dictionary<string, MainViewContext>();

        private int rows, columns;
        //Main view variables
        public List<string> ContextNames { get { return Contexts.Keys.ToList(); } }
        public ContextBasicData CurrentContext { get; set; }
        public ObservableCollection<ContextBasicData> Tabs { get; set; } = new ObservableCollection<ContextBasicData>();
        public string Name { get; set; }
        public ObservableCollection<FFBase> MasterViewList { get; set; }
        public Dictionary<string, ObservableCollection<FFBase>> Groups { get; set; }
        public ObservableCollection<BreadCrumb> BreadCrumbs { get; set; }
        public string MainViewAddress { get; set; }
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
                ResizeGrid(rows, CurrentContext.Columns);
                ArrangeIcons(CurrentContext.Rows, CurrentContext.Columns, CurrentContext);
            }
        }
        public int Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                ResizeGrid(CurrentContext.Rows, columns);
                ArrangeIcons(CurrentContext.Rows, CurrentContext.Columns, CurrentContext);
            }
        }

        public static Grid MainGrid { get; internal set; }

        //Methods
        public MainViewContext()
        {
            Groups = new Dictionary<string, ObservableCollection<FFBase>>() { };
            BreadCrumbs = new ObservableCollection<BreadCrumb>();
            CommonInstance = this;
            CurrentContext = new ContextBasicData();
            Tabs.Add(CurrentContext);
        }

        public void ArrangeIcons(int rows, int cols, ContextBasicData contextBasicData)
        {
            var tempList = new ObservableCollection<FFBase>();
            int row = 0, col;
            if (contextBasicData.Groups != null && contextBasicData.MasterViewList != null)
            {
                foreach (var group in contextBasicData.Groups)
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
                                contextBasicData.MainGrid.RowDefinitions.Add(new RowDefinition());
                            }
                            row++;
                        }
                    }
                    row++;
                }
                contextBasicData.MasterViewList = tempList;
            }
        }
        public void ResizeGrid(int rows, int cols)
        {
            if (CurrentContext.MainGrid == null)
                return;
            CurrentContext.MainGrid.ColumnDefinitions.Clear();
            CurrentContext.MainGrid.RowDefinitions.Clear();
            for (int i = 0; i < rows; i++)
            {
                CurrentContext.MainGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < cols; i++)
            {
                CurrentContext.MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
        public void Refresh(ContextBasicData context)
        {
            var di = new DirectoryInfo(MainViewAddress);
            var items = new ObservableCollection<FFBase>();
            foreach (var item in di.GetDirectories())
            {
                items.Add(new FFBase() { Name = item.Name, FullPath = item.FullName, Type = Type.Folder, DoubleClickIcon = () => { context.GetViewFromAddressString(item.FullName); }, SingleClickIcon = () => { } });
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
                    System.Windows.Media.Imaging.BitmapSource bitmapSource = WindowHelpers.GetBitmapSourceFromPath(item.FullPath);
                    item.Thumbnail = bitmapSource;
                }
                else
                {
                    item.Thumbnail = match.Thumbnail;
                }
            }
            MasterViewList = new ObservableCollection<FFBase>(items.ToList());
            Groups = new Dictionary<string, ObservableCollection<FFBase>> { { "Main", items } };
            ArrangeIcons(Rows, Columns, context);
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

        public static void AddNewMainViewData(string name)
        {
            var newContext = new ContextBasicData();
            newContext.Rows = 6;
            newContext.Columns = 15;
            newContext.GetViewFromAddressString("");
            MainViewContext.CommonInstance.Tabs.Add(newContext);
        }
        public void MainViewSelectionChanged(List<FFBase> fFBases)
        {
            UpdatePropertiesBox();
            UpdateContextMenuActions();
        }
        public void MainViewIconClicked(int X, int Y, System.Windows.Input.MouseButtonEventArgs e, FFBase ffbase = null, FrameworkElement control = null)
        {
            switch (e.ChangedButton)
            {
                case System.Windows.Input.MouseButton.Left:
                    //SingleClickIcon();
                    if (ffbase.isSecondClick)
                    {
                        ffbase.DoubleClickIcon();
                        ffbase.isSecondClick = false;
                        ffbase.timer.Stop();
                    }
                    else
                    {
                        ffbase.isSecondClick = true;
                        ffbase.timer.Start();
                    }
                    break;
                case System.Windows.Input.MouseButton.Middle:
                    break;
                case System.Windows.Input.MouseButton.Right:

                    var context = new CommandsMenuContext();
                    var commandsMenu = new Commands_Menu(context);
                    commandsMenu.Show();
                    ffbase.SetFFbaseActionsToMenuContext(commandsMenu.CommandsMenuContext);
                    SetMenuContextActions(commandsMenu.CommandsMenuContext, ffbase.ContextBasicData);
                    commandsMenu.Top = Y - commandsMenu.Height / 2 - 20;
                    commandsMenu.Left = X - commandsMenu.Width / 2 - 20;
                    break;
                case System.Windows.Input.MouseButton.XButton1:
                    break;
                case System.Windows.Input.MouseButton.XButton2:
                    break;
                default:
                    break;
            }
        }

        private void SetMenuContextActions(CommandsMenuContext commandsMenuContext, ContextBasicData context)
        {
            commandsMenuContext.AddCommand(new Command()
            {
                Name = "Big Icons",
                Action = (obj) =>
                {
                    MainViewContext.CommonInstance.Rows = 3;
                    MainViewContext.CommonInstance.Columns = 10;
                }
            }, new string[] { "View" });
            commandsMenuContext.AddCommand(new Command()
            {
                Name = "Small Icons",
                Action = (obj) =>
                {
                    MainViewContext.CommonInstance.Rows = 6;
                    MainViewContext.CommonInstance.Columns = 15;
                }
            }, new string[] { "View" });
            commandsMenuContext.AddCommand(new Command()
            {
                Name = "Group By Name",
                Action = (obj) =>
                {
                    CommonInstance.GroupBy((ffbase) => ffbase.Name.Substring(0, 1), (ffbases) => ffbases.Select(f => f.Name.Substring(0, 1)).Distinct(), CommonInstance.SortItemsFunction, context);
                }
            }, new string[] { "View", "Group By" });
            commandsMenuContext.AddCommand(new Command()
            {
                Name = "Group By Type",
                Action = (obj) =>
                {
                    CommonInstance.GroupBy((ffbase) => ffbase.Type.ToString(), (ffbases) => ffbases.Select(f => f.Type.ToString()).Distinct(), CommonInstance.SortItemsFunction, context);
                }
            }, new string[] { "View", "Group By" });
            commandsMenuContext.AddCommand(new Command()
            {
                Name = "Sort By Name",
                Action = (obj) =>
                {
                    CommonInstance.GroupBy(CommonInstance.GroupNamesFunc, CommonInstance.SortGroupsByNameFunction, (ffbases) => ffbases.OrderBy(x => x.Name), context);
                }
            }, new string[] { "View", "Sort By" });
            commandsMenuContext.AddCommand(new Command()
            {
                Name = "Sort By Size",
                Action = (obj) =>
                {
                    CommonInstance.GroupBy(CommonInstance.GroupNamesFunc, CommonInstance.SortGroupsByNameFunction, (ffbases) => ffbases.OrderBy(x => x.Size), context);
                }
            }, new string[] { "View", "Sort By" });
            commandsMenuContext.AddCommand(new Command()
            {
                Name = "Sort By Created",
                Action = (obj) =>
                {
                    CommonInstance.GroupBy(CommonInstance.GroupNamesFunc, CommonInstance.SortGroupsByNameFunction, (ffbases) => ffbases.OrderBy(x => x.Created), context);
                }
            }, new string[] { "View", "Sort By" });
            commandsMenuContext.AddCommand(new Command()
            {
                Name = "Sort By Modified",
                Action = (obj) =>
                {
                    CommonInstance.GroupBy(CommonInstance.GroupNamesFunc, CommonInstance.SortGroupsByNameFunction, (ffbases) => ffbases.OrderBy(x => x.LastModified), context);
                }
            }, new string[] { "View", "Sort By" });
        }

        private void UpdatePropertiesBox()
        {
            throw new NotImplementedException();
        }

        private void UpdateContextMenuActions()
        {
            throw new NotImplementedException();
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
            Func<IEnumerable<FFBase>, IEnumerable<FFBase>> sortItemsFunction,
            ContextBasicData context)
        {
            this.GroupNamesFunc = groupNameFunc;
            this.SortGroupsByNameFunction = sortGroupsByNameFunction;
            this.SortItemsFunction = sortItemsFunction;
            GetGroupBy(groupNameFunc, sortGroupsByNameFunction, sortItemsFunction, context);
        }
        public void GetGroupBy(Func<FFBase, string> groupNameFunc,
            Func<IEnumerable<FFBase>, IEnumerable<string>> sortGroupsByNameFunction,
            Func<IEnumerable<FFBase>, IEnumerable<FFBase>> sortItemsFunction,
            ContextBasicData context)
        {
            var groupNames = sortGroupsByNameFunction(context.MasterViewList);

            var tempGroups = new Dictionary<string, ObservableCollection<FFBase>>();
            foreach (var groupName in groupNames)
            {
                tempGroups.Add(groupName, new ObservableCollection<FFBase>(context.MasterViewList.Where(x => groupNameFunc(x).Equals(groupName))));
            }
            context.Groups = tempGroups;
            ArrangeIcons(Rows, Columns, context);
        }
        public void SortGroupNames(GroupSortBy groupSortBy, ContextBasicData context, bool Desc = false)
        {
            var groupticles = context.Groups.Keys.Select(v => (GroupName: v, GroupItems: Groups[v]));
            Dictionary<string, ObservableCollection<FFBase>> groups = new Dictionary<string, ObservableCollection<FFBase>>();
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
            context.Groups = groups;
            ArrangeIcons(Rows, Columns, CurrentContext);
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
                BreadCrumbs.Add(new BreadCrumb(breadCrumb, path, CurrentContext));
                BreadCrumbs.Add(new BreadCrumb($"{Path.DirectorySeparatorChar}", path, CurrentContext));
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
        public ContextBasicData Context { get; set; }

        public BreadCrumb(string caption, string fullPath, ContextBasicData context)
        {
            Caption = caption;
            FullPath = fullPath;
            Context = context;
        }
    }
}
