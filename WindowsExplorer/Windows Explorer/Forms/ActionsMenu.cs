using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Misc;

namespace Windows_Explorer.Forms
{
    public partial class ActionsMenu : Form
    {
        public FFBase FFBase { get; set; }
        public ActionsMenu(FFBase fFBase) : base()
        {
            InitializeComponent();
            FFBase = fFBase;
            Text = fFBase.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Context.GetItemsList(Context.CLIPBOARD).Add(FFBase);
            var lv = new Windows_Explorer.Forms.ListView(true, (string selectedListName) =>
            {
                Context.AddToList(new FFBaseCollection() { this.FFBase }, selectedListName);
            });
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
            if (response==DialogResult.Yes)
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
    }
}
