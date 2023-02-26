using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Shell;
using WindowsExplorer_WPF.Misc;
using WindowsExplorer_WPF.Misc.Helpers;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    internal class FileSystemPropertyFetcher : IPropertyQueryResultProvider
    {
        public FileSystemPropertyFetcher()
        {
            this.SupportedFields = new System.Collections.Generic.List<FieldName> {
                FieldName.Name, FieldName.Created, FieldName.Extension, FieldName.DefaultIcon, 
                FieldName.Thumbnail , FieldName.Size, FieldName.FullPath,FieldName.Location,
                FieldName.LastAccessed, FieldName.LastModified
            };
        }
        public override object GetProperty(string path, QueryObject queryObject)
        {
            var file = new FileInfo(path);
            ShellObject shellObject = null;
            BitmapSource bitmapSource;
            if (file.Exists)
            {
                switch (queryObject.FieldName)
                {
                    case FieldName.Name:
                        return file.Name;
                    case FieldName.Created:
                        return file.CreationTime;
                    case FieldName.Extension:
                        return file.Extension;
                    case FieldName.DefaultIcon:
                        shellObject = shellObject == null ? ShellObject.FromParsingName(path) : shellObject;
                        bitmapSource = MainViewDataHelpers.Bitmap2BitmapImage(shellObject.Thumbnail.Bitmap);
                        return bitmapSource;
                    case FieldName.Thumbnail:
                        shellObject = shellObject == null ? ShellObject.FromParsingName(path) : shellObject;
                        bitmapSource = MainViewDataHelpers.Bitmap2BitmapImage(shellObject.Thumbnail.Bitmap);
                        return bitmapSource;
                    case FieldName.Size:
                        return file.Length;
                    case FieldName.FullPath:
                        return file.FullName;
                    case FieldName.Location:
                        return file.DirectoryName;
                    case FieldName.LastModified:
                        return file.LastWriteTime;
                    case FieldName.LastAccessed:
                        return file.LastAccessTime;
                    default:
                        break;
                }
                return null;

            }
            else
            {
                return null;
            }
        }
    }
}
