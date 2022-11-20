using System.Resources;
using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Properties;

namespace Windows_Explorer.Misc.Property_Fetchers
{
    public class BasicFolderPropertiesFetcher
    {
        public static FileAndFolder.Folder CreateBasicFolderObject(string path)
        {
            var folder = new FileAndFolder.Folder();
            DirectoryInfo fileInfo = new DirectoryInfo(path);
            if (fileInfo.Exists)
            {
                folder.Name = fileInfo.Name;
                folder.Size = -1;
                folder.Type = FileAndFolder.Type.Folder;
                folder.LastAccessed = fileInfo.LastAccessTime;
                folder.LastModified = fileInfo.LastWriteTime;
                folder.FullPath = fileInfo.FullName;
                folder.IsFolder = true;
                folder.Created = fileInfo.CreationTime;
                folder.DefaultIcon = Resources.DefaultFolderIcon;

                folder.Attributes["Name"] = fileInfo.Name;
                folder.Attributes["Length"] = -1;
                folder.Attributes["File"] = folder.Type;
                folder.Attributes["LastAccessTime"] = fileInfo.LastAccessTime;
                folder.Attributes["LastModified"] = fileInfo.LastWriteTime;
                folder.Attributes["Created"] = fileInfo.CreationTime;
            }
            return folder;
        }

    }
}
