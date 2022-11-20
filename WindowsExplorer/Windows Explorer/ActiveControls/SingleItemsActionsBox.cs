using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Misc;


namespace Windows_Explorer.ActiveControls
{
    public partial class SingleItemsActionsBox : UserControl
    {
        public FFBase FFBase { get; set; }
        public SingleItemsActionsBox(FFBase fFBase)
        {
            InitializeComponent();
            FFBase = fFBase;
            Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Context.GetItemsList(Context.CLIPBOARD).Add(FFBase);
            var lv = new Windows_Explorer.Forms.ListView(true, (string selectedListName) =>
            {
                Context.AddToList(new FFBaseCollection() { this.FFBase }, selectedListName);
            });
        }

        private void button3_Click(object sender, EventArgs e)
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
}
