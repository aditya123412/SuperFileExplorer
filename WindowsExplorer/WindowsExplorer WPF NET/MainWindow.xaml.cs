using System.Linq;
using System.Collections.Generic;
using System.Windows;
using WindowsExplorer_WPF.Misc;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Timers;
using System;
using System.Windows.Controls;

namespace WindowsExplorer_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewData MainViewData { get; set; }
        public ObservableCollection<WindowsExplorer_WPF_NET.Misc.TreeNodeItem> TreeData { get; set; }
        public int ColumnCount { get; set; } = 16;
        public IEnumerable<FFBase> SelectedItems { get; private set; }

        public MainWindow()
        {
            //InitializeComponent();
            MainViewData = new MainViewData();

            this.DataContext = this;
            MainViewData.GetViewFromAddressString("");
            TreeData = MainViewData.TreeData;
        }


        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
            var crumb = ((FrameworkElement)sender).Tag as string;
            MainViewData.GetViewFromAddressString(crumb);
        }

        private void Copy(object sender, RoutedEventArgs e)
        {
        }

        private void Paste(object sender, RoutedEventArgs e)
        {

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
    }
}
