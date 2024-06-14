using Classes.Operations;
using System;
using System.IO;

namespace Classes.Common
{
    public class FileItem : IDescriptorNode
    {
        public FileItem(FileInfo fileInfo) : base(fullPath: fileInfo.FullName)
        {
            this.info = fileInfo;
            this.ParentPath = Directory.GetParent(fileInfo.FullName).FullName;
            this.type = DescriptorType.File;
            this.size = fileInfo.Length;
        }
        public string extension { get; set; } = "";
        public long size { get; set; } = 0;
        public bool isVirtual { get; set; } = false;
        public static FileItem GetFile(string path) { return null; }
        public new DataObject GetProperty(string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case Constants.EXTENSION:
                    return new DataObject((string)this.info.Extension, propertyName);
                case Constants.FILESIZE:
                    return new DataObject((float)this.size, propertyName);
                case Constants.CONTENT:
                    return new DataObject(Constants.CONTENT, DataObjectType.BLOB, null);
                case Constants.PREVIEW:
                    return new DataObject(Constants.PREVIEW, DataObjectType.BLOB, null);
                default:
                    return base.GetProperty(propertyName);
            }
        }

        public override void loadTagsAndProperties()
        {
            throw new NotImplementedException();
        }

        public override void saveTagsAndProperties()
        {
            throw new NotImplementedException();
        }
    }
}
