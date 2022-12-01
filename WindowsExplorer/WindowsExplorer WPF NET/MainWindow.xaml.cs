using System.Linq;
using System.Collections.Generic;
using System.Windows;
using WindowsExplorer_WPF.Misc;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Timers;
using System;

namespace WindowsExplorer_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer;
        bool isSecondClick = false;
        public MainViewData MainViewData { get; set; }
        public MainWindow()
        {
            //InitializeComponent();
            BrushConverter brushConverter = new BrushConverter();
            MainViewData = new MainViewData();

            this.DataContext = this;
            MainViewData.GetViewFromAddressString("C:\\");
        }


        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var icon = ((FrameworkElement)sender).DataContext as FFBase;
            if (icon != null)
            {
                icon.MouseDownAction(sender, e);
            }
        }
    }
}
