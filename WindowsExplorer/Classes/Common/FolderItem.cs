using Classes.Operations;
using System.Text.Json;
namespace Classes.Common
{
    public class FolderItem : IDescriptorNode
    {
        public static List<FolderProperties> GlobalPropertiesCollection = new List<FolderProperties>();
        static FolderItem()
        {
            var path = Path.Combine(Path.GetTempPath(), "Metadata");
            foreach (var item in new System.IO.DirectoryInfo(path).EnumerateFiles())
            {
                var val = System.IO.File.ReadAllText(item.FullName);
                var folderProps = JsonSerializer.Deserialize<FolderProperties>(val); 
                GlobalPropertiesCollection.Add(folderProps);
            }
        }
        public FolderItem(DirectoryInfo directoryInfo) : base(directoryInfo.FullName)
        {
            this.info = directoryInfo;
            this.ParentPath = Directory.GetParent(directoryInfo.FullName).FullName;
            this.type = DescriptorType.Directory;
        }

        //public new readonly DescriptorType type = DescriptorType.Directory;
        public IList<IDescriptorNode> Children;
        public bool isScriptOnly { get; set; } = false;
        private FolderProperties Properties { get; set; }
        public static FolderItem GetFolderItemFromDirectory(string path) { return null; }
        public static FolderItem GetFolderItemFromScript(string path) { return null; }
        public IList<IDescriptorNode> UpdateChildren()
        {
            Classes.Operations.ProjectAll projectAll = new Classes.Operations.ProjectAll();
            var results = projectAll.Execute(new Dictionary<string, DataObject> { { "Path", new DataObject("value", DataObjectType.String, ParentPath) } });
            this.Children = (IList<IDescriptorNode>)results.Value;
            return Children;
        }
        public new DataObject GetProperty(string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case Constants.FILESIZE:
                    return new DataObject(0, "NoSize");
                case Constants.CHILDREN:
                    return new DataObject((List<IDescriptorNode>)this.Children, propertyName);
                default:
                    return base.GetProperty(propertyName);
            }
        }

        public override void saveTagsAndProperties()
        {
            var path = Path.Combine(Path.GetTempPath(), "Metadata", Guid.NewGuid().ToString());
            System.IO.File.WriteAllText(path, JsonSerializer.Serialize(this.properties));
        }

        public override void loadTagsAndProperties()
        {

        }
    }
}
