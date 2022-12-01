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
        public List<FFBase> Items { get; set; }
        public ListItemsView(List<FFBase> fFBases):this(new FFBaseCollection(fFBases), Context.Main)
        {
            Items = fFBases;            
        }

        public ListItemsView(FFBaseCollection items, string groupName)
        {
            InitializeComponent();
            Items = items;
            panel1=  GridView.CreateViewGroup(panel1, new Dictionary<string, FFBaseCollection> { { groupName, new FFBaseCollection(Items)} });
            this.Text = groupName;
            ShowDialog();
        }

        private void ListItemsView_Load(object sender, EventArgs e)
        {
        }
    }
}
