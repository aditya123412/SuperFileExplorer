using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Forms;
using Type = Windows_Explorer.FileAndFolder.Type;

namespace Windows_Explorer.Misc
{
    public static class Context
    {
        private static List<string> _viewGroupNames = new List<string>();
        //List names
        public const string Main = "$MAIN";
        public const string Selected = "$MAIN";
        public static string WorkingListName = Main;
        public const string CLIPBOARD = "$CLIPBOARD";
        public const string BOOKMARKS = "$BOOKMARKS";
        public const string SENDTO = "$SENDTO";

        public static FFBaseCollection WorkingList { get; set; } = new FFBaseCollection();
        public static FFBase WorkingObject { get; set; }

        public static bool CtrlKeyPressed, AltKeyPressed, ShiftKeyPressed;
        public static MainWindow MainWindow;
        public static int CursorAt { get; set; } = 0;
        public static DirectoryInfo Directory { get; set; }
        public static string Path { get; set; }
        public static FFBaseCollection Files { get; set; } = new FFBaseCollection();
        public static Dictionary<string, FFBaseCollection> Lists { get; set; } = new Dictionary<string, FFBaseCollection> {
            { CLIPBOARD, new FFBaseCollection() },
            { BOOKMARKS, new FFBaseCollection() }
        };
        public static List<string> ViewGroupNames
        {
            get
            {
                return _viewGroupNames;
            }
            set
            {
                _viewGroupNames = value;
                OnViewGroupNamesChanged(value);
            }
        }

        public static IViewPanel MainPanel { get; set; }

        public static Action<List<string>> OnViewGroupNamesChanged { get; set; } = (x) => { };
        public static Func<FFBaseCollection, FFBaseCollection> MainViewFilter { get; set; } = (x) => x;
        public static Func<FFBaseCollection, FFBaseCollection> MainViewSort { get; set; } = (x) => x;
        public static Func<FFBaseCollection, Dictionary<string, FFBaseCollection>> MainViewGroupBy { get; set; } = (x) => { var groups = new Dictionary<string, FFBaseCollection>(); groups.Add(Main, x); return groups; };
        public static List<Func<string, object>> NavigationHandlers { get; set; } = new List<Func<string, object>>();
        public static List<Func<string, object>> ListCreationHandlers { get; set; }

        public static void SetContext(string Path, FFBaseCollection retrievedItems)
        {
            Context.Path = Path;
            if (!string.IsNullOrWhiteSpace(Path))
            {
                Context.Directory = new DirectoryInfo(Path);
            }
            Lists[Main] = retrievedItems;
            ViewGroupNames = new List<string> { Main };
            CursorAt = 0;
        }
        public static void GetContextItems(string path)
        {
            var directory = new DirectoryInfo(path);
            var openFSItems = new FFBaseCollection();

            foreach (var folder in directory.GetDirectories())
            {
                var item = MainFunctions.GetMetadata(folder.FullName, Type.Folder);
                openFSItems.Add(item);
                item.DataActions[FFBase.Click] = MainFunctions.FileClicked;
                item.DataActions[FFBase.DoubleClick] = MainFunctions.FileDoubleClicked;
            }

            foreach (var file in directory.GetFiles())
            {
                var item = MainFunctions.GetMetadata(file.FullName, Type.File);
                item.DataActions[FFBase.Click] = MainFunctions.FileClicked;
                item.DataActions[FFBase.DoubleClick] = MainFunctions.FileDoubleClicked;
                openFSItems.Add(item);
            }
            Context.ViewGroupNames = new List<string> { Main };
            Context.SetContext(path, openFSItems);
        }
        public static void GroupByName(FFBaseCollection fFBases)
        {
            const string FILES = "Files", FOLDERS = "Folders";

            if (!Context.ViewGroupNames.Contains(FOLDERS))
            {
                Context.ViewGroupNames.Add(FOLDERS);
            }
            if (!Context.ViewGroupNames.Contains(FILES))
            {
                Context.ViewGroupNames.Add(FILES);
            }
        }
        public static void Navigate(string path)
        {
            foreach (var navigationHandler in NavigationHandlers)
            {
                navigationHandler(path);
            }
        }
        public static void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                Context.CtrlKeyPressed = true;
                //MessageBox.Show("Ctrl key Down");
            }
            if (e.KeyCode == Keys.Menu || e.KeyCode == Keys.Alt)
            {
                Context.AltKeyPressed = true;
                //MessageBox.Show("Alt key Down");
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                Context.ShiftKeyPressed = true;
                //MessageBox.Show("Shift key Down");
            }
        }
        public static void KeyUpEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                Context.CtrlKeyPressed = false;
                //MessageBox.Show("Ctrl key up");
            }
            if (e.KeyCode == Keys.Menu || e.KeyCode == Keys.Alt)
            {
                Context.AltKeyPressed = false;
                //MessageBox.Show("Alt key up");
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                Context.ShiftKeyPressed = false;
                //MessageBox.Show("Shift key up");
            }
        }

        public static void AddToList(this FFBaseCollection items, string ListName, Action<FFBaseCollection> OnListChange = null, Action<string> IfListNotExist = null)
        {
            if (Lists.ContainsKey(ListName))
            {
                Lists[ListName].AddRange(items);
            }
            else
            {
                if (IfListNotExist != null)
                {
                    IfListNotExist(ListName);
                }
            }
        }
        public static void AddToNewList(this FFBaseCollection items, string ListName, Action<string> IfListAlreadyExist = null)
        {
            if (!Lists.ContainsKey(ListName))
            {
                Lists.Add(ListName, items);
            }
            else
            {
                if (IfListAlreadyExist != null)
                {
                    IfListAlreadyExist(ListName);
                }
            }
        }

        public static FFBaseCollection GetItemsList(string name, bool onlySelected = false, Action<string> IfListNotExist = null)
        {
            if (!Context.Lists.ContainsKey(name))
            {
                if (IfListNotExist != null)
                {
                    IfListNotExist(name);
                }
            }
            if (onlySelected)
            {
                return (FFBaseCollection)Context.Lists[name].Where(x => x.Selected);
            }
            return (FFBaseCollection)Context.Lists[name];
        }

        public static string CreateNewList(this FFBaseCollection items)
        {
            var newListName = Guid.NewGuid().ToString();
            Lists.Add(newListName, items);
            return newListName;
        }

        public static void DeleteList(string ListName, Action<string> IfListNotExist = null)
        {
            if (Lists.ContainsKey(ListName))
            {
                Lists.Remove(ListName);
            }
            else
            {
                if (IfListNotExist != null)
                {
                    IfListNotExist(ListName);
                }
            }
        }

        public static void RenameList(string listName, string newListname)
        {
            var temp = Lists[listName];
            Lists[newListname] = temp;
            Lists.Remove(listName);
        }
    }
}
