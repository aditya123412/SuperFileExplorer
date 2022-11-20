using System.Linq;
using Windows_Explorer.Misc;

namespace Windows_Explorer.Forms
{
    public partial class ListView : Form
    {
        private bool selectMode = false;
        private Action<string> action;
        public ListView(bool showSelectMode, Action<string> OnSelect)
        {
            this.selectMode = showSelectMode;
            this.action = OnSelect;
            InitializeComponent();
            Context.Lists.ToList().ForEach(list => listView1.Items.Add(list.Key));
        }

        private void ListView_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            action(listView1.SelectedItems[0].ToString());
        }
    }
}
