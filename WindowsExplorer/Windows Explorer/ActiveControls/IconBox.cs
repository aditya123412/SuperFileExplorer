using Classes.Operations;
using Windows_Explorer.FileAndFolder;
using DataObject = Classes.Operations.DataObject;
using Type = Windows_Explorer.FileAndFolder.Type;
using Windows_Explorer.Misc;

namespace Windows_Explorer.ActiveControls
{
    public partial class IconBox : ClickableItemBase
    {
        int _size;
        public Type _type;
        bool _selected;
        public Image _thumbnail;
        public FFBase fileItem;
        public int Index { get; set; }
        public List<string> Tags
        {
            get { return fileItem.Tags; }
            set { fileItem.Tags = value; }
        }

        //Names of FunctionDelegates that run with itself as the parameter
        public Dictionary<string, Func<ClickableItemBase, object>> IconBoxActions = new Dictionary<string, Func<ClickableItemBase, object>>();
        public Action<ClickableItemBase, Keys> KeyPress { get; set; }
        public Dictionary<string, object> Attributes
        {
            get { return fileItem.Attributes; }
            set { fileItem.Attributes = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (_selected)
                {
                    BackColor = System.Drawing.Color.LightSteelBlue;
                    this.BorderStyle = BorderStyle.FixedSingle;
                    Display.BorderStyle = BorderStyle.FixedSingle;
                }
                else
                {
                    BackColor = System.Drawing.Color.Transparent;
                    this.BorderStyle = BorderStyle.None;
                    Display.BorderStyle = BorderStyle.None;

                }
            }
        }

        public IconBox()
        {
            CreateNormalIcon();
        }

        public IconBox(FFBase data, int iconSize = 100, IconType iconType = IconType.Default)
        {
            fileItem = data;
            Data = data.FullPath;
            _type = data.Type;
            SetType(iconType);
            if (data.DefaultIcon != null)
            {
                Display.Image = data.DefaultIcon;
            }
            IconSize = iconSize;
            if (_type == Type.File)
            {
                Controls.Add(new Label() { Text = data.Size.ToString(), Top = Label.Bottom });
            }
            this.SetEventHandlers((x) =>
            {
                x.Click += new EventHandler(IconBox_Click);
                x.DoubleClick += new EventHandler(IconBox_DoubleClick);
            });
        }
        public IconBox(string label, int iconSize, Icon image, Type type, string path = "", string data = null)
        {
            this.fileItem = type == Type.Folder ? new FileAndFolder.Folder() { Name = label, DefaultIcon = image.ToBitmap(), Type = type } as FFBase :
                new FileAndFolder.File() { Name = label, DefaultIcon = image.ToBitmap(), Type = type } as FFBase;
            if (image != null)
            {
                Display.Image = image.ToBitmap();
                fileItem.Thumbnail = image.ToBitmap();
            }
            Data = data;
            _type = type;
            CreateNormalIcon();
            IconSize = iconSize;
            this.SetEventHandlers((x) =>
            {
                x.Click += new EventHandler(IconBox_Click);
                x.DoubleClick += new EventHandler(IconBox_DoubleClick);
            });
        }
        public int IconSize
        {
            get { return _size; }
            set
            {
                _size = value;
                this.Width = value + 2;
                Display.Height = Display.Width;
                Label.Top = value + 5; Label.Width = value;
                Label.Height = value / 3;
            }
        }
        public void SetType(IconType iconType)
        {
            switch (iconType)
            {
                case IconType.OnlyNames:
                    this.Controls.Clear();
                    this.CreateTextOnlyIcon();
                    break;
                case IconType.Default:
                    this.Controls.Clear();
                    this.CreateNormalIcon();
                    break;
                case IconType.OnlyThumbnails:
                    this.Controls.Clear();
                    this.CreateImageOnlyIcon();
                    break;
                default:
                    //this.Controls.Clear();
                    this.CreateNormalIcon();
                    break;
            }
        }

        private void IconBox_DoubleClick(object sender, EventArgs e)
        {
            if (fileItem.DataActions.ContainsKey(FFBase.DoubleClick) && fileItem.DataActions[FFBase.DoubleClick] != null)
            {
                fileItem.DataActions[FFBase.DoubleClick](fileItem);
            }
        }

        private void IconBox_Click(object sender, EventArgs e)
        {
            var me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {
            }
            else if (me.Button == MouseButtons.Left)
            {
                if (ClickableBaseActions.ContainsKey(FFBase.Click) && ClickableBaseActions[FFBase.Click] != null)
                    ClickableBaseActions[FFBase.Click](this);
                if (fileItem.DataActions.ContainsKey(FFBase.Click) && fileItem.DataActions[FFBase.Click] != null)
                    fileItem.DataActions[FFBase.Click](fileItem);
            }
            Selected = fileItem.Selected;
        }
        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
        }
        private void KeyUpEventHandler(object sender, KeyEventArgs e)
        {
        }
        private void IconBox_Load(object sender, EventArgs e)
        {

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (KeyPress != null)
            {
                KeyPress(this, keyData);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
    public enum IconType { Default, Details, OnlyThumbnails, OnlyNames }
}
