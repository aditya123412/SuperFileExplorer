using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Misc;

namespace Windows_Explorer.Forms
{
    public partial class ListItemsView : Form
    {
        public ListItemsView()
        {
            InitializeComponent();
        }
        public ListItemsView(FFBaseCollection items, string groupName)
        {
            InitializeComponent();
            //new GridView(0, 0, panel1.Width, panel1.Height, panel1, groupName, items);
        }

        private void ListItemsView_Load(object sender, EventArgs e)
        {

        }
    }
}
