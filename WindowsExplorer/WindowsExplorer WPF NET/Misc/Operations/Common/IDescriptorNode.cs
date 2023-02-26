using Classes.Operations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Common
{
    public abstract class IDescriptorNode
    {
        protected IDescriptorNode(string fullPath)
        {
            this.ParentPath = Directory.GetParent(fullPath).FullName;
            loadTagsAndProperties();
        }

        private string path;
        public FileSystemInfo info { get; set; }
        public string ParentPath { get; set; }
        public long Size { get; set; }
        public DescriptorType type { get; set; }
        public List<string> tags { get; set; }
        public Dictionary<string, Object> properties { get; set; }
        public void AddTag(string tag)
        {
            tags.Add(tag);
        }
        public abstract void saveTagsAndProperties();
        public abstract void loadTagsAndProperties();

        public void getTagsAndProperties()
        {
            tags = new List<string>();
            properties = new Dictionary<string, Object>();
        }
        public void RemoveTag(string tag)
        {
            if (tags.Contains(tag))
            {
                tags.Remove(tag);
            }
            saveTagsAndProperties();
        }
        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }
        public void SetProperty(string propertyName, Object propertyValue)
        {
            this.properties[propertyName] = propertyValue;
            saveTagsAndProperties();
        }
        public DataObject GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case Constants.FILENAME:
                    return new DataObject((string)this.info.Name, propertyName);
                case Constants.PARENT_DIRECTORY:
                    return new DataObject((string)this.ParentPath, propertyName);
                case Constants.CREATED:
                    return new DataObject((DateTime)this.info.CreationTime, propertyName);
                case Constants.MODIFIED:
                    return new DataObject((DateTime)this.info.LastWriteTime, propertyName);
                case Constants.LAST_ACCESSED:
                    return new DataObject((DateTime)this.info.LastAccessTime, propertyName);
                case Constants.TAGS:
                    return new DataObject((List<string>)this.tags, propertyName);
                default:
                    try
                    {
                        return new DataObject((string)this.properties[propertyName], propertyName);
                    }
                    catch (Exception)
                    {

                        return null;
                    }
            }
        }
        public void DeleteProperty(string propertyName)
        {
            if (properties.ContainsKey(propertyName))
            {
                properties.Remove(propertyName);
            }
            saveTagsAndProperties();
        }
        public enum DescriptorType
        {
            File = 0, Directory = 1, Script = 2, Snapshot = 3
        }
    }
}
