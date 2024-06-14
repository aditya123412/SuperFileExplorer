using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowsExplorer_WPF_NET.Controls
{
    /// <summary>
    /// Interaction logic for DynamicGrid.xaml
    /// </summary>
    public partial class DynamicGrid
    {
        public int ItemHeight { get; set; }
        public int ItemWidth { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public List<Control> Items { get; set; }
        public DynamicGrid(int itemWidth, int itemHeight, IEnumerable<Control> controls)
        {
            InitializeComponent();
            this.ItemWidth = itemWidth;
            this.ItemHeight = itemHeight;
            Items = controls.ToList();
        }
        private void SetColumnsByWidth()
        {
            ColumnCount = (int)(this.GridArea.Width / ItemWidth);
            this.GridArea.ColumnDefinitions.Clear();
            for (int i = 0; i < ColumnCount; i++)
            {
                GridArea.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}
