using Classes.Common;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Operations
{
    public class Merge : OperationImplementation
    {
        static OperationDescriptor descriptor = new OperationDescriptor("Merge",
            new Dictionary<string, DataObjectType> { { FILEGROUPS, DataObjectType.FileGroupsList } },
            DataObjectType.DescriptorNodeList);

        public static readonly string FILEGROUPS = "FileGroups";
        static Merge()
        {
            Register(new Merge());
        }
        public Merge()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var fileGroups = OperationExecutor.Evaluate(parameters[FILEGROUPS]).GetFileGroupsList();
            var fileList = DataObject.NewFileList();
            foreach (var fileGroup in fileGroups.Keys)
            {
                fileList = fileList.Concat(fileGroups[fileGroup]).ToList();
            }
            return new DataObject(fileList);
        }
    }
}
