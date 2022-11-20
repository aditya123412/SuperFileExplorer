using System.Diagnostics;
using Newtonsoft.Json;
using TreeDb;
using Windows_Explorer.ActiveControls;
using Windows_Explorer.Forms;
using Windows_Explorer.Misc.Property_Fetchers;
using Windows_Explorer.FileAndFolder;

namespace Windows_Explorer.Misc
{
    public static class MainFunctions
    {
        public static BasicFilePropertiesFetcher BasicFilePropertiesFetcher;
        public static BasicFolderPropertiesFetcher BasicFolderPropertiesFetcher;
        public static string ImageCachePath = Path.Combine(Application.UserAppDataPath, "IconsCache.json");

        public static void SetEventHandlers(this Control control, Action<Control> SetEventHandlersMethod)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }
            SetEventHandlersMethod(control);
            foreach (var cont in control.Controls)
            {
                (cont as Control).SetEventHandlers(SetEventHandlersMethod);
            }
        }

        public static object FileDoubleClicked(FFBase icon)
        {
            if (icon.Type==FileAndFolder.Type.Folder)
            {
                Context.Navigate(icon.FullPath);
            }
            if (icon.Type == FileAndFolder.Type.File)
            {
                Process.Start("explorer", "\"" + icon.FullPath + "\"");
            }
            return 0;
        }

        public static object FileClicked(FFBase _icon)
        {
            if (!Context.CtrlKeyPressed)
            {
                foreach (var item in Context.MainPanel.FSItems)
                {
                    item.Selected = false;
                }
                Context.Lists[Context.Selected] = new FFBaseCollection();
            }
            if (!_icon.Selected)
            {
                _icon.Selected = true;
            }
            return 0;
        }

        public static FileAndFolder.FFBase GetMetadata(string path, FileAndFolder.Type type)
        {
            if (type == FileAndFolder.Type.File)
            {
                return BasicFilePropertiesFetcher.CreateBasicFileObject(path);
            }
            else
            {
                return BasicFolderPropertiesFetcher.CreateBasicFolderObject(path);
            }
        }

        public static string ConvertPathToDirectoryQuery(this MainWindow mainWindow, string path)
        {
            return path;
        }
        public static bool ImageCacheExists(string path)
        {
            return System.IO.File.Exists(Path.Combine(path, "IconsCache.json"));
        }
        public static void SaveImageCache(Dictionary<string, Image> imageTuples, string path)
        {
            return;
            List<ImageTuple> images = new List<ImageTuple>();
            foreach (var imageTuple in imageTuples)
            {
                images.Add(new ImageTuple(imageTuple.Key, imageTuple.Value));
            }
            System.IO.File.WriteAllText(Path.Combine(path, "IconsCache.json"), JsonConvert.SerializeObject(images));
        }
        public static Dictionary<string, Image> LoadImageCache(string path)
        {
            var imageCache = new Dictionary<string, Image>();
            var imagedata = System.IO.File.ReadAllText(ImageCachePath);
            var imageTuples = JsonConvert.DeserializeObject<List<ImageTuple>>(Path.Combine(path, "IconsCache.json"));
            foreach (var item in imageTuples)
            {
                imageCache.Add(item.Key, item.Value);
            }
            return imageCache;
        }
    }
}
