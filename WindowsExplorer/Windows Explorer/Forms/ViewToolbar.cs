using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_Explorer.Misc;

namespace Windows_Explorer.Forms
{
    public partial class ViewToolbar : Form
    {
        public MainWindow mainWindow { get; }
        public ViewToolbar(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

        }

        private void IconType_CheckedChanged(object sender, EventArgs e)
        {
            switch ((sender as RadioButton).Tag)
            {
                case "OnlyImages":
                    (mainWindow.panel as GridView).MutateIcons(ActiveControls.IconType.OnlyThumbnails);
                    break;
                case "OnlyNames":
                    (mainWindow.panel as GridView).MutateIcons(ActiveControls.IconType.OnlyNames);
                    break;
                case "Details":
                    (mainWindow.panel as GridView).MutateIcons(ActiveControls.IconType.Details);
                    break;
                case "Icons":
                    (mainWindow.panel as GridView).MutateIcons(ActiveControls.IconType.Default);
                    break;
                default:
                    break;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            (mainWindow.panel as GridView).ResizeIcons(((int)numericUpDown1.Value));
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Save()
            Hide();
        }

        public void GroupBy(string property)
        {

        }
        public void SortBy(string property)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Name, (x1, x2) => String.Compare(x1, x2));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Size, (x1, x2) => { return (int)(x1 - x2); });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Type, (x1, x2) => String.Compare(x1.ToString(), x2.ToString()));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Tags, (x1, x2) => x1.Count - x2.Count);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Created, (x1, x2) => x1.Ticks > x2.Ticks ? 1 : -1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.LastModified, (x1, x2) => x1.Ticks > x2.Ticks ? 1 : -1);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var groups = Context.Lists[Context.Main].GroupBy(x => x.Name.Substring(0, 1));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var groups = Context.Lists[Context.Main].GroupBy(x => x.Type.ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var groups = Context.Lists[Context.Main].GroupBy(x => x.Created.Ticks);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var groups = Context.Lists[Context.Main].GroupBy(x => x.LastModified);
        }
    }
}
