using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Windows_Explorer.ActiveControls;
using Windows_Explorer.Misc;
using Microsoft.WindowsAPICodePack.Shell;
using Type = Windows_Explorer.ActiveControls.Type;

namespace Windows_Explorer.Forms
{
    public partial class ExplorerWindow : Form
    {
        int margin = 30;
        List<IconBox> icons = new List<IconBox>();
        Thread fetchThumbnailsThread;

        public ExplorerWindow()
        {
            InitializeComponent();
            fetchThumbnailsThread = new Thread(this.UpdateIconThumbnailsAsync);
        }
        private void FetchIcons()
        {
            if (fetchThumbnailsThread.ThreadState == System.Threading.ThreadState.Running)
            {
                fetchThumbnailsThread.Abort();
            }
            fetchThumbnailsThread = new Thread(this.UpdateIconThumbnailsAsync);
            fetchThumbnailsThread.Start();
        }
        private void SetIconAsync(IconBox icon, string path)
        {
            ShellObject shellObject = ShellObject.FromParsingName(path);
            var pic = shellObject.Thumbnail.Bitmap;
            pic.MakeTransparent();
            icon.Display.BackgroundImage = pic;
        }
        private void UpdateIconThumbnailsAsync()
        {
            icons.ForEach(icon =>
            {
                SetIconAsync(icon, icon.Data);
            });
        }
        private void ArrangeIcons()
        {
            int drawTop = margin;
            int drawLeft = 15;
            int drawableWidth = MainViewArea.Width - (2 * margin);

            icons.ForEach((iconBox) =>
            {
                iconBox.Top = drawTop;
                iconBox.Left = drawLeft;

                if (drawLeft + iconBox.Width < drawableWidth)
                {
                    drawLeft += iconBox.Width;
                }
                else
                {
                    drawLeft = 15;
                    drawTop += iconBox.Height;
                    if (drawTop + iconBox.Height > MainViewArea.Height)
                    {
                        MainViewArea.Height += iconBox.Height;
                    }
                }
            });
        }
        private object RenderFolderNew(string path)
        {
            var directory = new System.IO.DirectoryInfo(path);
            MainViewArea.Name = directory.Name;
            MainViewArea.Controls.Clear();
            icons.Clear();
            var nodesInfo = new List<System.IO.FileSystemInfo>();

            foreach (var folder in directory.GetDirectories())
            {
                IconBox iconBox = new IconBox(folder.Name, 100, null, Type.Folder, folder.FullName, folder.FullName);
                this.icons.Add(iconBox);
                nodesInfo.Add(folder);

                iconBox.Parent = this.MainViewArea;
                iconBox.DblClicked = RenderFolderNew;
            }

            foreach (var file in directory.GetFiles())
            {
                IconBox iconBox = new IconBox(file.Name, 100, null, Type.File, file.FullName, file.FullName);
                this.icons.Add(iconBox);
                nodesInfo.Add(file);

                iconBox.Parent = this.MainViewArea;
                iconBox.DblClicked = Process.Start;
            }
            ArrangeIcons();
            FetchIcons();
            return true;
        }

        private List<TreeNode> GetPreRenderMetadata(string path, IEnumerable<string> enumerable)
        {
            throw new NotImplementedException();
        }

        private void GetDrives()
        {
            var DrivesInfo = System.IO.DriveInfo.GetDrives();
            MainViewArea.Name = "My Computer";
            MainViewArea.Controls.Clear();
            icons.Clear();
            foreach (var drive in DrivesInfo)
            {
                IconBox driveIcon = new IconBox(drive.Name, 100, null, Type.Folder, $"{drive.Name}", $"{drive.Name}");
                this.icons.Add(driveIcon);

                driveIcon.Parent = this.MainViewArea;
                driveIcon.DblClicked = RenderFolderNew;
            }
            ArrangeIcons();
            FetchIcons();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            RectangleRounder.RectangleRound(MainViewArea, 40);
            RectangleRounder.RectangleRound(TopToolBar, 30);
            RectangleRounder.RectangleRound(button1, 25);
            RectangleRounder.RectangleRound(button2, 25);
            RectangleRounder.RectangleRound(button3, 25);
            RectangleRounder.RectangleRound(button4, 25);

            RenderMenuBar.Create(this.MainViewArea,
                new Dictionary<string, Delegate> { { "Test", null } },
                MenuOrientation.Vertical,
                200, 40, Color.Red, Color.DodgerBlue, Color.White);
            GetDrives();
        }

        private void ExplorerWindow_SizeChanged(object sender, EventArgs e)
        {
            ArrangeIcons();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            foreach (var icon in icons)
            {
                icon.IconSize = ((int)numericUpDown1.Value);
            }
            ArrangeIcons();
        }
    }
}
