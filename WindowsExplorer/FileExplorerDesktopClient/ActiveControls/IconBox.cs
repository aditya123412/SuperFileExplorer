using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;

namespace Windows_Explorer.ActiveControls
{
    public partial class IconBox : UserControl
    {
        String filePath;
        int _size;
        public Type _type;

        public Func<string, object> DblClicked;
        public string Data;
        public bool Selected;
        public ShellThumbnail _thumbnail { get { return _thumbnail; } }

        public IconBox()
        {
            InitializeComponent();
        }
        public IconBox(string label, int iconSize, System.Drawing.Icon image,Type type, string path = "", string data = null)
        {
            InitializeComponent();
            Display.Image = image.ToBitmap();
            Label.Text = label;
            IconSize = iconSize;
            filePath = path;
            Data = data;
            _type = type;
            Display.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }
        public int IconSize
        {
            get { return _size; }
            set { _size = value; Display.Width = value; Display.Height = value; this.Width = value + 2; Label.Top = value + 5; this.Width = value; }
        }
        public IconBox(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
        }

        private void IconBox_SizeChanged(object sender, EventArgs e)
        {
            _size = Width;
        }

        private void IconBox_DoubleClick(object sender, EventArgs e)
        {
            if (DblClicked != null)
                DblClicked(this.Data);
        }

        private void IconBox_Click(object sender, EventArgs e)
        {
            Selected = !Selected;
            if (Selected)
            {
                BackColor = System.Drawing.Color.LightSteelBlue;
            }
            else
            {
                BackColor = System.Drawing.Color.Transparent;
            }
        }
    }
    public enum Type { File, Folder}
}
