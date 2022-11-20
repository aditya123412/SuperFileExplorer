using System.IO;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Shell;
using TreeDb;
using TreeDb.Classes;
using Windows_Explorer.ActiveControls;
using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Misc;
using Type = Windows_Explorer.FileAndFolder.Type;

namespace Windows_Explorer.Forms
{
    public partial class MainWindow : Form
    {
        int margin = 30;
        int initHeight = 0;

        public bool CtrlKeyPressed, ShiftKeyPressed, AltKeyPressed;
        public bool MultiSelect { get; set; }
        public IViewPanel panel { get; private set; }

        public List<IconBox> icons = new List<IconBox>();

        public string ContextPath { get; set; }

        Thread fetchThumbnailsThread;
        FileSystemWatcher Watcher;

        private static readonly DatabaseInitConfig config = DatabaseInitConfig.LoadFromFile("");
        public Database db = new Database("FS", Path.Combine(Application.CommonAppDataPath));

        public MainWindow()
        {
            InitializeComponent();
            fetchThumbnailsThread = new Thread(this.UpdateThumbnailsAsync);
            db.SaveDatabaseToDisk(Path.Combine(Application.CommonAppDataPath, "dbconfig.json"));
            addressBarWithBreadCrumbs1.Navigate = RenderView;
            Context.NavigationHandlers.Add(RenderView);
        }
        private void FetchThumbnailStart()
        {
            if (fetchThumbnailsThread.ThreadState == System.Threading.ThreadState.Running)
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
                icons.ForEach((Action<IconBox>)(icon =>
                {
                    this.SetThumbnailAsync(icon, icon.Data);
                }));
            }
            catch (Exception e)
            {
            }
        }
        private void SetThumbnailAsync(IconBox icon, string path)
        {
            ShellObject shellObject = ShellObject.FromParsingName(path);
            var pic = shellObject.Thumbnail.Icon.ToBitmap();
            pic.MakeTransparent();
            icon.Display.Image = pic;
            icon.Invalidate();
        }

        public void ArrangeIcons()
        {
            int paneTop = MainViewArea.Top;
            initHeight = MainViewArea.Height;

            int drawTop = margin;
            int drawLeft = 15;
            int drawableWidth = MainViewArea.Width - (2 * margin);
            MainViewArea.Height = initHeight;

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
                }
            });
        }

        //public void GetContextItems(string path)
        //{
        //    var directory = new DirectoryInfo(path);
        //    var openFSItems = new List<FFBase>();
        //    const string FILES = "Files", FOLDERS = "Folders";
        //    Context.Lists.Add(FOLDERS, new List<FFBase>());
        //    foreach (var folder in directory.GetDirectories())
        //    {
        //        var item = this.GetMetadata(folder.FullName, Type.Folder);
        //        item.OnDblClick = this.FileDoubleClicked;
        //        openFSItems.Add(item);
        //        Context.Lists[FOLDERS].Add(item);
        //    }
        //    Context.Lists.Add(FILES, new List<FFBase>());
        //    foreach (var file in directory.GetFiles())
        //    {
        //        var item = this.GetMetadata(file.FullName, Type.File);
        //        item.OnDblClick = this.FileDoubleClicked;
        //        openFSItems.Add(item);
        //        Context.Lists[FILES].Add(item);
        //    }
        //    Context.ViewGroupNames.Add(FOLDERS);
        //    Context.ViewGroupNames.Add(FILES);
        //    Context.SetContext (path, openFSItems);
        //}

        public object RenderView(string path)
        {
            ContextPath = path;
            try
            {
                switch (path)
                {
                    case "":
                        this.RenderHomeView();
                        return true;
                    case "[Some Custom Path]":
                        //Some custom render logic
                        return true;
                        break;
                    default:

                        Context.GetContextItems(path);
                        MainViewArea.Name = Context.Directory.Name;
                        MainViewArea.Controls.Clear();
                        this.addressBarWithBreadCrumbs1.Text = path;
                        icons.Clear();

                        panel = Misc.GridView.CreateViewGroup(MainViewArea, Context.ViewGroupNames.Select(name => (name, Context.GetItemsList(name, false))).ToList());

                        Context.MainPanel = panel;

                        Watcher = new FileSystemWatcher(path);
                        Watcher.Changed += new FileSystemEventHandler((s, e) =>
                        {
                            Context.GetContextItems(path);
                            MainViewArea.Name = Context.Directory.Name;
                            MainViewArea.Controls.Clear();

                            icons.Clear();
                            //Context.Lists[Context.Main].Clear();

                            var nodesInfo = new List<FileSystemInfo>();

                            panel = Misc.GridView.CreateViewGroup(8, 8, MainViewArea.Width - 8, MainViewArea.Height - 8,
                                MainViewArea, Context.Directory.Name, Context.Lists[Context.Main]);
                        });
                        return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void RenderHomeView()
        {
            try
            {
                var DrivesInfo = DriveInfo.GetDrives();
                MainViewArea.Name = "My Computer";
                MainViewArea.Controls.Clear();
                icons.Clear();
                Context.Path = "";
                var drives = new List<FFBase>();
                foreach (var drive in DrivesInfo)
                {
                    var driveInfo = new FFBase() { Type = Type.Folder, Name = drive.Name, Location = "", FullPath = drive.Name };
                    driveInfo.DataActions[FFBase.DoubleClick] = MainFunctions.FileDoubleClicked;
                    drives.Add(driveInfo);

                    //IconBox driveIcon = new IconBox(drive.Name, 100, null, Type.Folder, $"{drive.Name}", $"{drive.Name}");
                    //this.icons.Add(driveIcon);

                    //driveIcon.Parent = this.MainViewArea;
                    //driveIcon.fileItem.FullPath = drive.Name;
                    //driveIcon.fileItem.Type = Type.Folder;
                    //driveIcon.fileItem.DataActions[FFBase.DoubleClick] = MainFunctions.FileDoubleClicked;
                }
                Context.SetContext("", new FFBaseCollection(drives));
                panel = Misc.GridView.CreateViewGroup(MainViewArea, Context.ViewGroupNames.Select(name => (name, Context.GetItemsList(name, false))).ToList());
                Context.MainPanel = panel;

                //ArrangeIcons();
                //FetchThumbnailStart();
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            //RectangleRounder.RectangleRound(MainViewArea, 40);
            RectangleRounder.RectangleRound(TopToolBar, 30);
            RectangleRounder.RectangleRound(button1, 25);
            RectangleRounder.RectangleRound(button2, 25);
            RectangleRounder.RectangleRound(button3, 25);
            RectangleRounder.RectangleRound(button4, 25);

            RenderMenuBar.Create(this.MainViewArea,
                new Dictionary<string, Delegate> { { "Test", null } },
                MenuOrientation.Vertical,
                200, 40, Color.Red, Color.DodgerBlue, Color.White);
            RenderHomeView();
        }

        private void ExplorerWindow_SizeChanged(object sender, EventArgs e)
        {
            ArrangeIcons();
        }

        private void addressBarWithBreadCrumbs1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewToolbar v = new ViewToolbar(this);
            v.Show();
        }
    }
}
