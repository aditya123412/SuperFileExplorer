using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Eto.IO;
using Microsoft.WindowsAPICodePack.Shell;
using Windows_Explorer.ActiveControls;
using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Windows_Explorer.Misc
{
    public class GridView : IViewPanel
    {
        public Panel mainPanel;
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
        public int IconSize { get; set; }

        public GridView(Panel container, Dictionary<string, FFBaseCollection> itemGroups, bool scrollBars = true, int iconSize = 100)//:this(container, itemGroups.ToDictionary(x=>x.Key, x => x.Value.Select(y=>new IconBox(y) as ClickableItemBase).ToList()))
        {
            FSItems = new List<FFBase>();

            this.IconSize = iconSize;

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

                        icon.ClickableBaseActions[FFBase.Click] = (ClickableItemBase cb) =>
                        {
                            if (!MultiSelect)
                            {
                                UnselectAll();
                            }
                            CursorPosition = icon.Index;
                            (cb as IconBox).Selected = true;
                            icon.Focus(); return null;
                        };
                        icon.SetEventHandlers(x =>
                        {
                            x.KeyDown += new KeyEventHandler((obj, args) => { Key_Down(args); });
                            x.KeyUp += new KeyEventHandler((obj, args) => { Key_Up(args); });
                            x.KeyPress += new KeyPressEventHandler((obj, args) => { Key_Press(args); });
                            x.Click += new EventHandler((o, e) => { RightClickHandler(icon, e); });
                        });
                        icon.KeyPress = (icon, key) => { Key_Down(new KeyEventArgs(key)); };

                        icon.Index = index++;
                        Items.Add(icon);
                        groupItems.Add(icon);
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
        }
        public void RenderGridView(Panel container, Dictionary<string, List<ClickableItemBase>> itemGroups, bool scrollBars = true)
        {
            mainPanel = container;
            mainPanel.AutoScroll = scrollBars;
            mainPanel.AutoSize = !scrollBars;
            Groups = itemGroups;
            Draw();
            if (itemGroups.SelectMany(x=>x.Value.ToList()).Count()>0)
            {
                Groups.First().Value.First().Focus();
            }
        }

        public GridView(Panel container, List<(string groupName, FFBaseCollection GroupList)> itemGroups, bool scrollBars = true, int iconSize = 100) : this(container, itemGroups.ToDictionary(x => x.groupName, x =>
            {
                return x.GroupList;
            }), scrollBars, iconSize)
        {
        }

        public void Draw()
        {
            mainPanel.Controls.Clear();
            previousRowbottom = 0;
            mainPanel.Controls.Clear();
            Rows.Clear();
            BackColor = Color.Red;
            WinApi.MakeTransparent(this.Handle);
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
                groupBodyRow.SuspendLayout();
                groupBodyRow.Paint += new PaintEventHandler((o, e) =>
                {
                });

                int drawTop = OuterMargin;
                int drawLeft = OuterMargin;
                foreach (var cib in Groups[group])
                {
                    var icon = (IconBox)cib;

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
                groupBodyRow.ResumeLayout(false);
                mainPanel.Controls.Add(groupBodyRow);
                drawTop = label.Top + label.Height + OuterMargin;
            }
        }

        private void RightClickHandler(object? sender, EventArgs e)
        {
            var me = e as MouseEventArgs;
            var iconBox = sender as IconBox;
            if (me.Button == MouseButtons.Right)
            {
                ActionsMenu actionMenu = new ActionsMenu(iconBox.fileItem);
                actionMenu.Show();
                actionMenu.Focus();

                actionMenu.Top = Cursor.Position.Y;
                actionMenu.Left = Cursor.Position.X;
                this.Refresh();
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
                    int destUp = Math.Max(0, CursorPosition - Rows.First().Count - 1);
                    if (keyEventArgs.Shift)
                    {
                        for (int i = destUp; i < CursorPosition; i++)
                        {
                            Items[i].Selected = true;
                        }
                    }
                    Items[destUp].Selected = true;
                    Items[destUp].Focus();
                    CursorPosition = destUp;
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
                    if (keyEventArgs.Shift)
                    {
                        for (int i = CursorPosition; i < destDown; i++)
                        {
                            Items[i].Selected = true;
                        }
                    }
                    Items[destDown].Selected = true;
                    Items[destDown].Focus();
                    CursorPosition = destDown;
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
                case Keys.Apps:
                    break;
                case Keys.OemMinus:
                    ResizeIcons(this.IconSize -= 10);
                    break;
                case Keys.Oemplus:
                    ResizeIcons(this.IconSize += 10);
                    break;
                case Keys.ControlKey:
                    MultiSelect = true;
                    break;
                case Keys.Enter:
                    MainFunctions.FileDoubleClicked(Items[CursorPosition].fileItem);
                    break;
                default:
                    break;
            }
            if (keyEventArgs.KeyCode >= Keys.A && keyEventArgs.KeyCode <= Keys.Z)
            {
                UnselectAll();
                for (int i = CursorPosition; i < Items.Count; i++)
                {
                    if (Items[i].fileItem.Name[0] == keyEventArgs.KeyValue)
                    {
                        Items[i].Selected = true;
                        CursorPosition = i;
                        break;
                    }
                }
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
            return null; //new GridView(panelConfig.Top, panelConfig.Left, panelConfig.Width, panelConfig.Height, panelConfig.ParentView, panelConfig.Title, panelConfig.Files, panelConfig.Margin, panelConfig.Gap, panelConfig.ScrollBars);
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
            return CursorPosition;
        }

        public override void SetCursorPosition(int position)
        {
            this.CursorPosition = position;
            Items[CursorPosition].Selected = true;
        }

        public FFBaseCollection GetSelectedItems()
        {
            return new FFBaseCollection(Items.Where(item => item.Selected).Select(x => x.fileItem));
        }

        public static GridView CreateViewGroup(Panel mainViewArea, Dictionary<string, FFBaseCollection> items)
        {
            return new GridView(mainViewArea, items);
        }
        public static GridView CreateViewGroup(Panel container, List<(string groupName, FFBaseCollection fileItems)> items)
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
                for (int i = 0; i < Items.Count; i++)
                {
                    var icon = Items.ElementAt(i);
                    if (icon.fileItem.Thumbnail == null)
                    {
                        this.SetThumbnailAsync(icon, icon.Data);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        private void SetThumbnailAsync(IconBox icon, string path)
        {
            ShellObject shellObject = ShellObject.FromParsingName(path);
            var pic = shellObject.Thumbnail.Bitmap;
            icon.fileItem.Thumbnail = pic;

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
}
