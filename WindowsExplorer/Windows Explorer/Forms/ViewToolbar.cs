using System.Data;
using Windows_Explorer.ActiveControls;
using Windows_Explorer.Misc;
using GridView = Windows_Explorer.Misc.GridView;

namespace Windows_Explorer.Forms
{
    public partial class ViewToolbar : Form
    {
        private int Reverse = 1;
        public MainWindow mainWindow { get; }
        public ViewToolbar(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            BackColor = SystemColors.ActiveBorder;
            this.Leave += new EventHandler((o, e) =>
            {
                Close();
            });
            this.LostFocus += new EventHandler((o, e) =>
            {
                Close();
            });
            Focus();
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
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Save()
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Name, (x1, x2) => String.Compare(x1, x2) * Reverse);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Size, (x1, x2) => { return (int)(x1 - x2) * Reverse; });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Type, (x1, x2) => String.Compare(x1.ToString(), x2.ToString()) * Reverse);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Tags, (x1, x2) => (x1.Count - x2.Count) * Reverse);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.Created, (x1, x2) => (x1.Ticks > x2.Ticks ? 1 : -1) * Reverse);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Context.Lists[Context.Main].Sort((x) => x.LastModified, (x1, x2) => (x1.Ticks > x2.Ticks ? 1 : -1) * Reverse);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var gridview = Context.MainPanel as GridView;
            var items = gridview.Groups.SelectMany(x => x.Value);

            Dictionary<string, List<ActiveControls.ClickableItemBase>> iconGroups = new Dictionary<string, List<ActiveControls.ClickableItemBase>>();
            foreach (IconBox item in items)
            {
                var key = item.fileItem.Name.Substring(0, 1);
                if (!iconGroups.ContainsKey(key))
                {
                    iconGroups.Add(key, new List<ActiveControls.ClickableItemBase> { item });
                }
                else
                {
                    iconGroups[key].Add(item);
                }

            }
            gridview.RenderGridView(gridview.mainPanel, iconGroups);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var groups = Context.Lists[Context.Main].GroupBy(x => x.Type.ToString());
            Groupify(groups);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var groups = Context.Lists[Context.Main].GroupBy(x => x.Created.Year.ToString());
            Groupify(groups);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var groups = Context.Lists[Context.Main].GroupBy(x => x.LastModified.Month.ToString());
            Groupify(groups);
        }

        private void SortDescebdubgCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Reverse = SortDescebdubgCheckBox.Checked ? -1 : 1;
        }

        static void Groupify(Dictionary<string, FFBaseCollection> groups)
        {
            Context.ViewGroupNames.Clear();
            foreach (var group in groups)
            {
                Context.AddToNewList(group.Value, group.Key);
                Context.ViewGroupNames.Add(group.Key);
            }
            Context.MainWindow.SetWindowGridItems(Context.ViewGroupNames.Select(name => (name, Context.GetItemsList(name))).ToList());
        }
    }
}
