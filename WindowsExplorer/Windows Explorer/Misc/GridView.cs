using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Windows_Explorer.ActiveControls;
using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Forms;

namespace Windows_Explorer.Misc
{
    public class GridView : IViewPanel
    {
        private Panel mainPanel;
        private int drawTop;
        private int previousRowbottom = 0;

        public bool MultiSelect { get; set; }
        public int HorizontalGap { get; set; } = 8;
        public int VerticalGap { get; set; } = 8;
        public int OuterMargin { get; set; } = 8;

        private Thread fetchThumbnailsThread;
        private Thread fetchIconsThread;
        public List<IconBox> Items = new List<IconBox>();
        public List<List<ClickableItemBase>> Rows = new List<List<ClickableItemBase>>();
        public Func<(string path, FileAndFolder.Type type, Func<IconBox, object> OnClicked, Func<IconBox, object> OnDblClicked), UserControl> GetUserControl { get; set; }
        public Dictionary<string, List<ClickableItemBase>> Groups { get; set; } = new Dictionary<string, List<ClickableItemBase>>();

        public GridView(int top, int left, int width, int height, Control parent, string Title,
                        FFBaseCollection items,
                        int margin = 30, int gap = 0, bool scrollBars = false)
        {
            mainPanel = new Panel();
            mainPanel.Parent = parent;
            mainPanel.Top = top;
            mainPanel.Left = left;
            mainPanel.Width = width;
            mainPanel.Height = height;
            mainPanel.Margin = new Padding(0);
            mainPanel.Padding = new Padding(0);
            mainPanel.AutoScroll = scrollBars;
            mainPanel.AutoSize = !scrollBars;

            Label label = new Label();
            label.Text = Title;
            label.Parent = mainPanel;
            label.Top = 15; label.Left = 25;

            this.Parent = parent;
            this.FSItems = items;

            int index = 0;
            foreach (var item in items)
            {
                var icon = new IconBox(item, 100);
                icon.Parent = mainPanel;
                icon.Key_Down = Key_Down;
                icon.Key_Up = Key_Up;
                icon.fileItem.DataActions[FFBase.Click] = (FFBase f) => { CursorPosition = index; return null; };

                icon.Index = index++;
                Items.Add(icon);
            }
            drawTop = label.Top + label.Height + margin;
            ArrangeIcons(margin, gap, gap);

            fetchThumbnailsThread = new Thread(this.UpdateThumbnailsAsync);

            FetchThumbnailStart();
        }
        public GridView(Panel container, Dictionary<string, FFBaseCollection> itemGroups, bool scrollBars = true)//:this(container, itemGroups.ToDictionary(x=>x.Key, x => x.Value.Select(y=>new IconBox(y) as ClickableItemBase).ToList()))
        {
            FSItems = new List<FFBase>();

            int index = 0;
            Groups.Clear();
            foreach (var group in itemGroups.ToList())
            {
                if (group.Value.Count > 0)
                {

                    FSItems.AddRange(group.Value);
                    var groupItems = new List<ClickableItemBase>();

                    foreach (var item in group.Value)
                    {
                        var icon = new IconBox(item);

                        icon.Index = index++;
                        Items.Add(icon);
                        groupItems.Add(icon);
                        icon.Parent = container;
                    }
                    Groups.Add(group.Key, groupItems);
                    group.Value.OnOrderChange = (FFlist) => { Draw(); };
                    group.Value.OnItemsChange = (FFlist) => { Draw(); };
                    group.Value.OnRemoveItem = (FFlist) => { Draw(); };
                    group.Value.OnAdd = (FFlist) => { Draw(); };
                    group.Value.OnItemChange = (FFlist) => { Draw(); };

                }
            }
            fetchIconsThread = new Thread(this.UpdateIconsAsync);
            FetchIconsStart();
            fetchThumbnailsThread = new Thread(this.UpdateThumbnailsAsync);
            FetchThumbnailStart();
            RenderGridView(container, Groups);
            Groups.First().Value.First().Focus();
        }
        public void RenderGridView(Panel container, Dictionary<string, List<ClickableItemBase>> itemGroups, bool scrollBars = true)
        {
            mainPanel = container;
            mainPanel.AutoScroll = scrollBars;
            mainPanel.AutoSize = !scrollBars;

            Draw();
        }

        public GridView(Panel container, List<(string groupName, FFBaseCollection GroupList)> itemGroups, bool scrollBars = true) : this(container, itemGroups.ToDictionary(x => x.groupName, x =>
            {
                return x.GroupList;
            }), scrollBars)
        {
            //FSItems = new List<FFBase>();

            //int index = 0;
            //Groups.Clear();
            //foreach (var group in itemGroups)
            //{
            //    if (group.GroupList.Count > 0)
            //    {

            //        FSItems.AddRange(group.GroupList);
            //        var groupItems = new List<ClickableItemBase>();

            //        foreach (var item in group.GroupList)
            //        {
            //            var icon = new IconBox(item);

            //            icon.Index = index++;
            //            Items.Add(icon);
            //            groupItems.Add(icon);
            //        }
            //        Groups.Add(group.groupName, groupItems);
            //        group.GroupList.OnOrderChange = (FFlist) => { Draw(); };
            //        group.GroupList.OnItemsChange = (FFlist) => { Draw(); };
            //        group.GroupList.OnRemoveItem = (FFlist) => { Draw(); };
            //        group.GroupList.OnAdd = (FFlist) => { Draw(); };
            //        group.GroupList.OnItemChange = (FFlist) => { Draw(); };
            //    }
            //}
            //fetchThumbnailsThread = new Thread(this.UpdateThumbnailsAsync);
            //FetchThumbnailStart();
            //RenderGridView(container, Groups);
        }

        public void Draw()
        {
            previousRowbottom = 0;
            mainPanel.Controls.Clear();
            Rows.Clear();

            int drawableWidth = mainPanel.Width - (2 * OuterMargin);

            var row = new List<ClickableItemBase>();
            foreach (var group in Groups.Keys)
            {
                var groupHeadingRow = CreateRow(mainPanel);
                Label label = new Label() { Text = group, Left = 25, Top = 10, Margin = new Padding(5) };
                Label status = new Label() { Text = group, Left = 500, Top = 10, Margin = new Padding(5) };
                groupHeadingRow.Controls.Add(label);
                groupHeadingRow.Controls.Add(status);
                groupHeadingRow.BackColor = Color.LightGray;
                groupHeadingRow.Top = previousRowbottom;

                previousRowbottom = groupHeadingRow.Bottom;
                mainPanel.Controls.Add(groupHeadingRow);

                var groupBodyRow = CreateRow(mainPanel);

                int drawTop = OuterMargin;
                int drawLeft = OuterMargin;
                foreach (var cib in Groups[group])
                {
                    var icon = (IconBox)cib;
                    icon.Key_Down = Key_Down;
                    icon.Key_Up = Key_Up;
                    icon.ClickableBaseActions[FFBase.Click] = (ClickableItemBase cb) =>
                    {
                        if (!MultiSelect)
                        {
                            UnselectAll();
                        }
                        CursorPosition = icon.Index;
                        (cb as IconBox).Selected = true; status.Text = icon.Index.ToString();
                        icon.Focus(); return null;
                    };
                    icon.SetEventHandlers(x =>
                    {
                        x.KeyDown += new KeyEventHandler((obj, args) => { status.Text = MultiSelect.ToString(); Key_Down(args); });
                        x.KeyUp += new KeyEventHandler((obj, args) => { status.Text = MultiSelect.ToString(); Key_Up(args); });
                        x.KeyPress += new KeyPressEventHandler((obj, args) => { Key_Press(args); });
                    });
                    icon.KeyPress = (icon, key) => { Key_Down(new KeyEventArgs(key)); };

                    if ((drawLeft + icon.Width + HorizontalGap < drawableWidth) && (!icon.FullWidth))
                    {
                        icon.Top = drawTop;
                        icon.Left = drawLeft;
                        row.Add(icon);
                        drawLeft += icon.Width + HorizontalGap;
                    }
                    else
                    {
                        drawLeft = OuterMargin;
                        drawTop += icon.Height + VerticalGap;
                        Rows.Add(row);
                        row = new List<ClickableItemBase>();
                        icon.Top = drawTop;
                        icon.Left = drawLeft;
                        row.Add(icon);
                    }

                    groupBodyRow.Controls.Add(cib);
                }
                Rows.Add(row);

                groupBodyRow.Top = previousRowbottom;
                previousRowbottom = groupBodyRow.Bottom;
                mainPanel.Controls.Add(groupBodyRow);
                drawTop = label.Top + label.Height + OuterMargin;
            }
        }

        public object Key_Press(KeyPressEventArgs keyEventArgs)
        {
            switch (keyEventArgs.KeyChar)
            {
                case ((char)Keys.Home):
                    UnselectAll();
                    CursorPosition = 0;
                    this.Items.First().Selected = true;
                    break;
                case (char)Keys.End:
                    UnselectAll();
                    CursorPosition = Items.Count - 1;
                    this.Items.Last().Selected = true;
                    break;
            }
            return true;
        }
        public object Key_Down(KeyEventArgs keyEventArgs)
        {
            Context.CtrlKeyPressed = keyEventArgs.Control;
            Context.AltKeyPressed = keyEventArgs.Alt;
            Context.ShiftKeyPressed = keyEventArgs.Shift;

            int pos;
            switch (keyEventArgs.KeyCode)
            {
                case Keys.Up:
                    pos = 0;
                    Items.ForEach(item =>
                    {
                        if (item.Selected)
                        {
                            CursorPosition = pos;
                        };
                        if (!keyEventArgs.Shift)
                        {
                            item.Selected = false;
                        }
                        pos++;
                    });
                    int dest = Math.Max(0, CursorPosition - Rows.First().Count - 1);
                    if (keyEventArgs.Shift)
                    {
                        for (int i = dest; i < CursorPosition; i++)
                        {
                            Items[Math.Max(0, dest)].Selected = true;
                            Items[Math.Max(0, dest)].Focus();
                        }
                    }
                    Items[Math.Max(0, dest)].Selected = true;
                    Items[Math.Max(0, dest)].Focus();
                    CursorPosition = dest;
                    break;
                case Keys.Down:
                    pos = 0;
                    Items.ForEach(item =>
                    {
                        if (item.Selected)
                        {
                            CursorPosition = pos;
                        };
                        if (!keyEventArgs.Shift)
                        {
                            item.Selected = false;
                        }
                        pos++;
                    });
                    int destDown = Math.Min(Items.Count - 1, CursorPosition + Rows.First().Count + 1);
                    CursorPosition = destDown;
                    Items[Math.Max(0, destDown)].Selected = true;
                    Items[Math.Max(0, destDown)].Focus();

                    break;
                case Keys.Left:
                    pos = 0;
                    Items.ForEach(item =>
                    {
                        if (item.Selected)
                        {
                            CursorPosition = pos;
                        };
                        if (!keyEventArgs.Shift)
                        {
                            item.Selected = false;
                        }
                        pos++;
                    });
                    Items[Math.Max(0, CursorPosition - 1)].Selected = true;
                    Items[Math.Max(0, CursorPosition - 1)].Focus();
                    break;
                case Keys.Right:
                    pos = 0;
                    Items.ForEach(item =>
                    {
                        if (item.Selected)
                        {
                            CursorPosition = pos;
                        };
                        if (!keyEventArgs.Shift)
                        {
                            item.Selected = false;
                        }
                        pos++;
                    });
                    Items[Math.Min(Items.Count - 1, CursorPosition + 1)].Selected = true;
                    Items[Math.Min(Items.Count - 1, CursorPosition + 1)].Focus();
                    break;
                case Keys.Home:
                    UnselectAll();
                    this.Items.First().Selected = true;
                    break;
                case Keys.End:
                    UnselectAll();
                    this.Items.Last().Selected = true;
                    break;
                case Keys.ControlKey:
                    MultiSelect = true;
                    break;
                default:
                    break;
            }
            if (keyEventArgs.KeyCode >= Keys.A && keyEventArgs.KeyCode <= Keys.Z)
            {

            }
            return 0;
        }
        public object Key_Up(KeyEventArgs keyEventArgs)
        {
            Context.CtrlKeyPressed = !keyEventArgs.Control;
            Context.AltKeyPressed = !keyEventArgs.Alt;
            Context.ShiftKeyPressed = !keyEventArgs.Shift;

            switch (keyEventArgs.KeyCode)
            {
                case Keys.Up:
                    break;
                case Keys.Down:
                    break;
                case Keys.Left:
                    break;
                case Keys.Right:
                    break;
                case Keys.Home:
                    break;
                case Keys.End:
                    break;
                case Keys.ControlKey:
                    MultiSelect = false;
                    break;
                default:
                    break;
            }
            return 0;
        }

        private Panel CreateRow(Control parent)
        {
            Panel panel = new Panel();
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowOnly;
            panel.Margin = new Padding(0, 0, 0, 0);
            panel.Padding = new Padding(0, 0, 0, 0);
            panel.Height = 1;
            panel.Width = parent.Width;
            panel.Visible = true;
            parent.Controls.Add(panel);
            return panel;
        }

        public void ArrangeIcons(List<ClickableItemBase> iconBoxes)
        {

            drawTop = 0;

            int drawLeft = OuterMargin;
            int drawableWidth = mainPanel.Width - (2 * OuterMargin);
            var row = new List<ClickableItemBase>();

            iconBoxes.ForEach((iconBox) =>
            {
                //iconBox.Top = drawTop;
                //iconBox.Left = drawLeft;
                //row.Add(iconBox);

                if ((drawLeft + iconBox.Width + HorizontalGap < drawableWidth) && (!iconBox.FullWidth))
                {
                    iconBox.Top = drawTop;
                    iconBox.Left = drawLeft;
                    row.Add(iconBox);
                    drawLeft += iconBox.Width + HorizontalGap;
                }
                else
                {
                    drawLeft = OuterMargin;
                    drawTop += iconBox.Height + VerticalGap;
                    //mainPanel.Height += iconBox.Height + VerticalGap;
                    Rows.Add(row);
                    row = new List<ClickableItemBase>();

                    iconBox.Top = drawTop;
                    iconBox.Left = drawLeft;
                    row.Add(iconBox);
                }
            });
            Rows.Add(row);
        }

        public void ArrangeIcons(int margin, int HorizontalGap, int VerticalGap)
        {
            int drawLeft = margin;
            int drawableWidth = mainPanel.Width - (2 * margin);

            Items.ForEach((iconBox) =>
            {
                iconBox.Top = drawTop;
                iconBox.Left = drawLeft;

                if ((drawLeft + iconBox.Width + HorizontalGap < drawableWidth) && (!iconBox.FullWidth))
                {
                    drawLeft += iconBox.Width + HorizontalGap;
                }
                else
                {
                    drawLeft = margin;
                    drawTop += iconBox.Height + VerticalGap;
                    mainPanel.Height += iconBox.Height + VerticalGap;
                }
            });
        }

        public void ResizeIcons(int Size)
        {
            foreach (var item in Items)
            {
                item.IconSize = Size;
            }
            ArrangeIcons(30, 0, 0);
        }

        public void MutateIcons(IconType iconType)
        {
            foreach (var item in Items)
            {
                item.SetType(iconType);
            }
            ArrangeIcons(30, 0, 0);
        }

        public override IViewPanel Get(PanelConfig panelConfig)
        {
            return new GridView(panelConfig.Top, panelConfig.Left, panelConfig.Width, panelConfig.Height, panelConfig.ParentView, panelConfig.Title, panelConfig.Files, panelConfig.Margin, panelConfig.Gap, panelConfig.ScrollBars);
        }

        public override List<ClickableItemBase> GetSelected()
        {
            return Items.Where(item => item.Selected).Select(x => x as ClickableItemBase).ToList();
        }

        public override void UnselectAll()
        {
            this.Items.ForEach(x => { x.Selected = false; });
        }

        public override void SelectAll()
        {
            this.Items.ForEach(x => { x.Selected = true; });
        }

        public override int GetCursorPosition()
        {
            return 0;
        }

        public override void SetCursorPosition(int position)
        {
            throw new NotImplementedException();
        }

        internal static GridView CreateViewGroup(int v1, int v2, int v3, int v4, Panel mainViewArea, string name, FFBaseCollection items)
        {
            return new GridView(v1, v2, v3, v4, mainViewArea, name, items);
        }
        internal static GridView CreateViewGroup(Panel mainViewArea, Dictionary<string, FFBaseCollection> items)
        {
            return new GridView(mainViewArea, items);
        }
        internal static GridView CreateViewGroup(Panel container, List<(string groupName, FFBaseCollection fileItems)> items)
        {
            return new GridView(container, items);
        }
        private void FetchIconsStart()
        {
            //if (fetchIconsThread.ThreadState == System.Threading.ThreadState.Running)
            //{
            //    fetchIconsThread.Interrupt();
            //}
            //fetchIconsThread = new Thread(this.UpdateIconsAsync);
            //fetchIconsThread.Start();
            //this.UpdateIconsAsync();
        }
        private void UpdateIconsAsync()
        {
            try
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    var icon = Items[i];
                    //this.SetIconsAsync(icon, icon.Data);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private void FetchThumbnailStart()
        {
            if (fetchIconsThread.ThreadState == System.Threading.ThreadState.Running)
            {
                fetchThumbnailsThread.Interrupt();
            }
            fetchThumbnailsThread = new Thread(this.UpdateThumbnailsAsync);
            fetchThumbnailsThread.Start();
        }
        private void UpdateThumbnailsAsync()
        {
            try
            {
                //if (MainFunctions.ImageCacheExists(Context.Path))
                //{
                //    ImageCache = MainFunctions.LoadImageCache(Context.Path);
                //}
                //else
                //{
                //    ImageCache = new Dictionary<string, Image>();
                //}
                //bool cacheInvalidated = false;
                //Items.ForEach(x =>
                //{
                //    if (ImageCache.ContainsKey(x.Name))
                //    {
                //        x.Display.Image = ImageCache[x.Name];
                //    }
                //    else
                //    {
                //        ShellObject shellObject = ShellObject.FromParsingName(x.fileItem.FullPath);
                //        var pic = shellObject.Thumbnail.Bitmap;
                //        pic.MakeTransparent();
                //        ImageCache.Add(x.Name, pic);
                //        x.Display.Image = pic;
                //        cacheInvalidated = true;
                //    }
                //});
                //if (cacheInvalidated)
                //    MainFunctions.SaveImageCache(ImageCache, Context.Path);

                //else
                //{
                for (int i = 0; i < Items.Count; i++)
                {
                    var icon = Items.ElementAt(i);
                    this.SetThumbnailAsync(icon, icon.Data);
                }
                //}
            }
            catch (Exception e)
            {
            }
        }
        private void SetThumbnailAsync(IconBox icon, string path)
        {
            ShellObject shellObject = ShellObject.FromParsingName(path);
            var pic = shellObject.Thumbnail.Bitmap;
            pic.MakeTransparent();
            icon.Display.Image = pic;
        }
        private void SetIconsAsync(IconBox icon, string path)
        {
            ShellObject shellObject = ShellObject.FromParsingName(path);
            var pic = shellObject.Thumbnail.MediumIcon.ToBitmap();
            pic.MakeTransparent();
            icon.Display.Image = pic;
        }

    }
    public enum ViewType
    {
        Icons, Rows, Columns
    }
    public class GridItem : ClickableItemBase
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Index { get; set; }
        public GridItem() : base()
        {
            this.KeyPress += new KeyPressEventHandler(this.OnGridItemKeyPress);
        }
        void OnGridItemKeyPress(object sender, KeyPressEventArgs e)
        {
            if (true)
            {

            }
        }
    }
}
