using WindowsExplorer_WPF_NET.Misc.Data;
using System.Windows.Input;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using WindowsExplorer_WPF.Misc;
using WindowsExplorer_WPF_NET.Misc;
using System.Windows.Data;

namespace WindowsExplorer_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, ObservableCollection<FFBase>> Lists = new Dictionary<string, ObservableCollection<FFBase>>();
        private readonly string CLIPBOARD = "$CLIPBOARD";

        public CollectionViewSource csv;

        public MainViewData MainViewData { get; set; }
        public int ColumnCount { get; set; } = 16;

        public MainWindow()
        {
            //InitializeComponent();
            MainViewData = new MainViewData(CreateRowAndColumnDefinitions);
            MainViewData.Rows = 6;
            MainViewData.Columns = 15;
            this.DataContext = this;
            MainViewData.GetViewFromAddressString("");

        }

        public void CreateRowAndColumnDefinitions(int rows, int columns)
        {
            for (int i = 0; i < columns; i++)
            {
                // Create column definitions
                //GroupsList.
            }
            for (int i = 0; i < rows; i++)
            {
                // Create row definitions
            }
        }

        private void Icon_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameworkElement senderElement = (FrameworkElement)sender;
            var icon = senderElement.DataContext as FFBase;

            if (icon != null)
            {
                var relativePosition = e.GetPosition(senderElement);
                var point = senderElement.PointToScreen(relativePosition);
                icon.MouseDownAction(sender, e, ((int)point.X), ((int)point.Y));
            }
        }

        private void HomeButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainViewData.GetViewFromAddressString("");
            GroupsList.Focus();
        }

        private void BreadCrumbClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var crumbAddressInTag = ((FrameworkElement)sender).Tag as string;
            MainViewData.GetViewFromAddressString(crumbAddressInTag);
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            Lists[CLIPBOARD] = new ObservableCollection<FFBase>(MainViewData.MainViewSelected);
        }

        private void Paste(object sender, RoutedEventArgs e)
        {
            string destFolder;
            if (MainViewData.MainViewSelected.Count() == 1 && MainViewData.MainViewSelected.First().Type == Misc.Type.Folder)
            {
                destFolder = MainViewData.MainViewSelected.First().FullPath;
            }
            else
            {
                destFolder = MainViewData.MainViewAddress;
            }
            foreach (var item in Lists[CLIPBOARD])
            {
                switch (item.Type)
                {
                    case Misc.Type.File:
                        System.IO.File.Copy(item.FullPath, Path.Combine(destFolder, item.Name));
                        break;
                    case Misc.Type.Folder:
                        CopyDirectoryRecursive(item.FullPath, MainViewData.MainViewSelected.First().FullPath);
                        break;
                    case Misc.Type.Any:
                        break;
                    case Misc.Type.CustomScript:
                        break;
                    default:
                        break;
                }
            }
            MainViewData.Refresh();
        }

        private object CopyDirectoryRecursive(string SourceFolderName, string DestFolderName)
        {
            dynamic result = null;
            var dir = new DirectoryInfo(SourceFolderName);

            foreach (var item in dir.GetDirectories())
            {
                CopyDirectoryRecursive(item.FullName, Path.Combine(DestFolderName, item.Name));
            }
            foreach (var item in dir.GetFiles())
            {
                System.IO.File.Copy(item.FullName, DestFolderName);
            }
            return result;
        }

        private void Rename(object sender, RoutedEventArgs e)
        {

        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var response = System.Windows.Forms.MessageBox.Show($"Do you want to delete the selcted ({MainViewData.MainViewSelected.Count()}) items?", "Delete items", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (response == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (var item in MainViewData.MainViewSelected)
                {
                    switch (item.Type)
                    {
                        case Misc.Type.File:
                            System.IO.File.Delete(item.FullPath);
                            break;
                        case Misc.Type.Folder:
                            System.IO.Directory.Delete(item.FullPath);
                            break;
                        case Misc.Type.Any:
                            break;
                        case Misc.Type.CustomScript:
                            break;
                        default:
                            break;
                    }
                }
            }
            MainViewData.Refresh();
        }

        private void GroupsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ItemsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            foreach (FFBase item in list.Items)
            {
                item.Selected = false;
            }
            foreach (FFBase item in list.SelectedItems)
            {
                item.Selected = true;
            }
        }

        private void GroupGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ColumnCount = (int)(MainWindowGrid.ColumnDefinitions[1]).Width.Value / 120;
        }

        private void GroupBy(object sender, RoutedEventArgs e)
        {
            var sortByField = FieldName.Type;
            var maxSubStringLength = 5;

            System.Func<IEnumerable<FFBase>, IEnumerable<string>> sortGroupsByNameFunction = (IEnumerable<FFBase> groupNames) => groupNames.OrderBy(x => x[sortByField])
            .Select(x => x[sortByField].ToString().Substring(0, System.Math.Min(maxSubStringLength, x[sortByField].ToString().Length - 1))).Distinct().Reverse();

            System.Func<FFBase, string> groupNameFunc = (FFBase ffbase) => ffbase[sortByField].ToString()
            .Substring(0, System.Math.Min(maxSubStringLength, ffbase[sortByField].ToString().Length - 1));

            System.Func<IEnumerable<FFBase>, IEnumerable<FFBase>> sortItemsFunction = (IEnumerable<FFBase> ffbases) => ffbases;

            MainViewData.GroupBy(groupNameFunc, sortGroupsByNameFunction, sortItemsFunction);
        }

        private void SortBy(object sender, RoutedEventArgs e)
        {
            MainViewData.SortWithinGroup(WindowsExplorer_WPF_NET.Misc.Data.FieldName.Size);
        }

        private void ResizeMainGridWidth(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int colCount = (int)IconSizeSlider.Value;
            SetMainViewColumnCount(colCount);
        }

        private void SetMainViewColumnCount(int colCount)
        {
            UniformGrid ug = GetAllGroupGrids();
            if (ug != null)
            {
                ug.Columns = colCount;
            }
        }

        UniformGrid GetAllGroupGrids()
        {
            return WindowHelpers.FindChild<UniformGrid>(MainWindowGrid, "GroupGrid");
        }

        private void ScrollViewer_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            MainScrollableArea.ScrollToVerticalOffset(MainScrollableArea.VerticalOffset - e.Delta);
        }

        private void TreeItemClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var treeNode = (TreeNodeItem)((StackPanel)sender).DataContext;
            if (treeNode != null)
            {
                switch (e.ChangedButton)
                {
                    case System.Windows.Input.MouseButton.Left:
                        MainViewData.GetViewFromAddressString(treeNode.FullPath);
                        break;
                    case System.Windows.Input.MouseButton.Middle:
                        break;
                    case System.Windows.Input.MouseButton.Right:
                        // Code to show right click menu
                        break;
                    case System.Windows.Input.MouseButton.XButton1:
                        break;
                    case System.Windows.Input.MouseButton.XButton2:
                        break;
                    default:
                        break;
                }
            }
        }

        private void TreeView_Expanded(object sender, RoutedEventArgs e)
        {
            var treeNode = (TreeNodeItem)((TreeViewItem)e.OriginalSource).DataContext;
            if (treeNode != null)
            {
                if (MainViewData.MainViewAddress == treeNode.FullPath)
                {
                    return;
                }
                MainViewData.SetTreeViewItems(treeNode.FullPath);
            }
        }

        private void GroupName_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var _sender = (TextBlock)sender;
            if (_sender != null)
            {
                var parentList = WindowHelpers.FindParent<DependencyObject>(_sender, "GroupParent") as StackPanel;
                var list = WindowHelpers.FindChild<DependencyObject>(parentList, "ItemsList") as ListView;
                list.SelectAll();
            }
        }

        private void GroupGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var _sender = (ListView)sender;
            if (_sender != null)
            {
                _sender.UnselectAll();
            }
        }

        private void GroupsList_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var _sender = (ListView)sender;
            var list = WindowHelpers.FindChildren<List<DependencyObject>>(_sender, "ItemsList");
            foreach (var item in list)
            {
                ((ListView)item).UnselectAll();
            }
        }

        private void MainScrollableArea_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var lists = WindowHelpers.FindChildren<List<DependencyObject>>(MainScrollableArea, "ItemsList");
            foreach (var item in lists)
            {
                ((ListView)item).UnselectAll();
            }
        }

        private void CloseMainWindow(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SortByTotalSize(object sender, RoutedEventArgs e)
        {
            MainViewData.SortGroupNames(MainViewData.Groups, GroupSortBy.TotalSize);
        }

        private void SortByGroupCount(object sender, RoutedEventArgs e)
        {
            MainViewData.SortGroupNames(MainViewData.Groups, GroupSortBy.Count);
        }

        private void SortByGroupLatestItem(object sender, RoutedEventArgs e)
        {
            MainViewData.SortGroupNames(MainViewData.Groups, GroupSortBy.NewestItem);
        }

        private void SortByGroupNames(object sender, RoutedEventArgs e)
        {
            MainViewData.SortGroupNames(MainViewData.Groups, GroupSortBy.Name);
        }


        private void MainViewArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                var list = WindowHelpers.FindChildren<List<DependencyObject>>(MainScrollableArea, "ItemsList");
                foreach (var item in list)
                {
                    ((ListView)item).SelectAll();
                }
            }
            if (e.Key == Key.Back)
            {

            }
        }


        private void NewGroupBy_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(GroupsList.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
            view.GroupDescriptions.Add(groupDescription);
        }
    }
    class FFBaseEqualityComparer : IEqualityComparer<FFBase>
    {
        public bool Equals(FFBase x, FFBase y)
        {
            return x.FullPath.Equals(y.FullPath, System.StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(FFBase obj)
        {
            return obj.FullPath.GetHashCode();
        }
    }
}
