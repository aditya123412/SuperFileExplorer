using System.Linq;
using System.Collections.Generic;
using System.Windows;
using WindowsExplorer_WPF.Misc;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Timers;
using System;
using System.Windows.Controls;
using System.IO;
using WindowsExplorer_WPF_NET.Misc;
using System.Windows.Controls.Primitives;

namespace WindowsExplorer_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, ObservableCollection<FFBase>> Lists = new Dictionary<string, ObservableCollection<FFBase>>();
        private readonly string CLIPBOARD = "$CLIPBOARD";

        public MainViewData MainViewData { get; set; }
        public int ColumnCount { get; set; } = 16;
        public IEnumerable<FFBase> SelectedItems { get; private set; }

        public MainWindow()
        {
            //InitializeComponent();
            MainViewData = new MainViewData(CreateRowAndColumnDefinitions);

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
            var icon = ((FrameworkElement)sender).DataContext as FFBase;
            if (icon != null)
            {
                icon.MouseDownAction(sender, e);
            }
        }

        private void HomeButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainViewData.GetViewFromAddressString("");
            this.GroupsList.Focus();
        }

        private void TreeviewNode_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void BreadCrumbClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var crumbAddresInTag = ((FrameworkElement)sender).Tag as string;
            MainViewData.GetViewFromAddressString(crumbAddresInTag);
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
            Lists[CLIPBOARD] = new ObservableCollection<FFBase>(SelectedItems);
        }

        private void Paste(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count() == 1 && SelectedItems.First().Type == Misc.Type.Folder)
            {
                foreach (var item in Lists[CLIPBOARD])
                {
                    switch (item.Type)
                    {
                        case Misc.Type.File:
                            System.IO.File.Copy(item.FullPath, SelectedItems.First().FullPath);
                            break;
                        case Misc.Type.Folder:
                            CopyDirectoryRecursive(item.FullPath, SelectedItems.First().FullPath);
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

        }

        private void GroupsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ItemsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var list = sender as ListView;

            foreach (FFBase item in list.SelectedItems)
            {
                item.Selected = true;
            }
            SelectedItems = this.MainViewData.Groups.SelectMany(x => x.Value.Where(y => y.Selected));
        }

        private void GroupGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ColumnCount = (int)(MainWindowGrid.ColumnDefinitions[1]).Width.Value / 120;
        }

        private void GroupBy(object sender, RoutedEventArgs e)
        {
            MainViewData.GroupBy(WindowsExplorer_WPF_NET.Misc.Data.FieldName.Type);
        }

        private void SortBy(object sender, RoutedEventArgs e)
        {
            MainViewData.Sort(WindowsExplorer_WPF_NET.Misc.Data.FieldName.Size);
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
            return FindChild<UniformGrid>(MainWindowGrid, "GroupGrid");
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

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
