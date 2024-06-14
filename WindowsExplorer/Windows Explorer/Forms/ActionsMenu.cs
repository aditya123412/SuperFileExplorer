using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Misc;

namespace Windows_Explorer.Forms
{
    public partial class ActionsMenu : Form
    {
        public FFBase FFBase { get; set; }
        public ActionsMenu(FFBase fFBase) : base()
        {
            FFBase = fFBase;
            Text = fFBase.Name;
            InitializeComponent();
            Focus();

            this.SetAsDestinationBtn.Visible = FFBase.Type == FileAndFolder.Type.Folder;
            foreach (Control control in Controls)
            {
                control.Click += new EventHandler((o, e) => { Close(); });
            }
            Leave+=new EventHandler((o, e) => { Close(); });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Context.GetItemsList(Context.CLIPBOARD).Add(FFBase);

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Context.GetItemsList(Context.CLIPBOARD).Add(FFBase);
            var lv = new Windows_Explorer.Forms.ListView(false, (string selectedListName) =>
            {
                Context.AddToList(new FFBaseCollection() { this.FFBase }, selectedListName);
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var response = MessageBox.Show($"Are you sure you want to delete {FFBase.Name}?", "Delete Items", MessageBoxButtons.YesNo);
            if (response == DialogResult.Yes)
            {
                if (FFBase.Type == FileAndFolder.Type.File)
                {
                    System.IO.File.Delete(FFBase.FullPath);
                }
                if (FFBase.Type == FileAndFolder.Type.Folder)
                {
                    System.IO.Directory.Delete(FFBase.FullPath);
                }
            }
        }

        private void ActionsMenu_Leave(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void SetAsDestinationBtn_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var lb = new ListView(true, (string selectedListName) =>
             {
                 var destList = Context.Lists[selectedListName];
                 destList.Add(this.FFBase);
             });
            
        }
    }
}
