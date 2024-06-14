using Classes.Common;

namespace Classes.Operations
{
    public class ProjectAll : OperationImplementation
    {
        public static readonly string PATH = "Path";

        static OperationDescriptor descriptor = new OperationDescriptor("ProjectAll",
            new Dictionary<string, DataObjectType> { { PATH, DataObjectType.String } },
            DataObjectType.DescriptorNodeList);
        static ProjectAll()
        {
            Register(new ProjectAll());
        }
        public ProjectAll()
        {
            this.Descriptor = descriptor;
        }

        public DataObject Execute(Dictionary<string, DataObject> parameters)
        {
            string basePath = ((string)parameters[PATH].Value).Replace(@"\\", @"\");

            var FilesAndFolders = ProjectAll.EvaluateNative(basePath);
            return new DataObject("Nodes", DataObjectType.DescriptorNodeList, FilesAndFolders);
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            string basePath = OperationExecutor.operationImplementations[parameters[PATH].OperationName]
                .Evaluate(parameters[PATH].Arguments)
                .GetString();

            var FilesAndFolders = ProjectAll.EvaluateNative(basePath);
            return new DataObject("Nodes", DataObjectType.DescriptorNodeList, FilesAndFolders);
        }

        public new void Register1()
        {
            //ScriptExecutor.engine.SetValue("ProjectAll", ProjectAll.EvaluateNative);
        }
        public static IEnumerable<IDescriptorNode> EvaluateNative(string path)
        {
            string basePath = path.Replace(@"\\", @"\");

            System.IO.DirectoryInfo directoryInfo = new DirectoryInfo(basePath);
            var FilesAndFolders = DataObject.NewFileList();

            foreach (var folder in directoryInfo.EnumerateDirectories())
            {
                var folderItem = new FolderItem(folder);
                FilesAndFolders?.Add(folderItem);
            }
            foreach (var file in directoryInfo.EnumerateFiles().Where(x => !x.Extension.Equals("fscr")))
            {
                var fileItem = new FileItem(file);
                FilesAndFolders?.Add(fileItem);
            }
            return FilesAndFolders;
        }
    }
}
