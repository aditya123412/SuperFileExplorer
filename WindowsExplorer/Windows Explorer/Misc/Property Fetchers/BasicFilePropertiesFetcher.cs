using Windows_Explorer.FileAndFolder;

namespace Windows_Explorer.Misc.Property_Fetchers
{
    public class BasicFilePropertiesFetcher
    {
        //public static ExtensionsConfig extensionsConfig = ExtensionsConfig.Load(Path.Combine(Application.CommonAppDataPath, "ExtensionConfig.json"));
        public static DefaultIconsCache defaultIconsCache = new DefaultIconsCache();
        public static FileAndFolder.File CreateBasicFileObject(string path, bool getAdditionalProps = false)
        {
            var file = new FileAndFolder.File();
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                file.Name = fileInfo.Name;
                file.Extension = fileInfo.Extension;
                file.Size = fileInfo.Length;
                file.Type = FileAndFolder.Type.File;
                file.LastAccessed = fileInfo.LastAccessTime;
                file.LastModified = fileInfo.LastWriteTime;
                file.Created = fileInfo.CreationTime;
                file.FullPath = fileInfo.FullName;
                file.IsFolder = false;
                file.DefaultIcon = defaultIconsCache.GetIcon(file.Extension, path).ToBitmap();

                file.Attributes["Name"] = fileInfo.Name;
                file.Attributes["Extension"] = fileInfo.Extension;
                file.Attributes["Length"] = fileInfo.Length;
                file.Attributes["File"] = FileAndFolder.Type.File;
                file.Attributes["LastAccessTime"] = fileInfo.LastAccessTime;
                file.Attributes["LastModified"] = fileInfo.LastWriteTime;
                file.Attributes["Created"] = fileInfo.CreationTime;
            }
            if (getAdditionalProps)
            {

            }
            return file;
        }

    }

}
