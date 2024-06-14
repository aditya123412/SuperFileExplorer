using System.Linq;
using Windows_Explorer.Misc;

namespace Windows_Explorer.Forms
{
    public partial class ListView : Form
    {
        private bool selectMode = false;
        private Action<string> action;
        public ListView(bool showSelectMode, Action<string> OnSelect = null)
        {
            this.selectMode = showSelectMode;
            this.action = OnSelect;
            InitializeComponent();
            Context.Lists.ToList().ForEach(list => listView1.Items.Add(list.Key));
            Show();
            button3.Visible = selectMode;
        }

        private void ListView_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectMode)
            {
                action(listView1.SelectedItems[0].Text);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new ListItemsView(Context.Lists[listView1.SelectedItems[0].Text]);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FFBaseCollection newList = new FFBaseCollection()
            {
                Name = textBox1.Text
            };

            if (Persistent.Checked)
            {
                newList.OnItemsChange = (f) =>
                             {
                                 System.IO.File.WriteAllText(Path.Combine(Application.UserAppDataPath, f.Name),
                                     f.ToString());
                             };
            }

            Context.AddToNewList(newList, newList.Name);
            listView1.Items.Add(new ListViewItem(textBox1.Text));
        }
    }
}
