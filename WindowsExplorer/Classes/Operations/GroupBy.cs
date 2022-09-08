using Classes.Common;

namespace Classes.Operations
{
    public class GroupBy : OperationImplementation
    {
        static readonly string GROUPBY = "GroupBy";
        static readonly string FILES = "Files";
        static readonly string FIELD = "FIELD";

        static OperationDescriptor descriptor = new OperationDescriptor(GROUPBY,
            new Dictionary<string, DataObjectType> { { FILES, DataObjectType.DescriptorNodeList },
                { FIELD, DataObjectType.String } },
                DataObjectType.FileGroupsList);
        static GroupBy()
        {
            Register(new GroupBy());
        }
        public GroupBy()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var fieldName = parameters[FIELD].evaluate().ToString();
            var groups = DataObject.NewFileGroup();
            var files = parameters[FILES].evaluate().GetDescriptorNodes();
            foreach (var file in files)
            {
                var fieldValue = file.GetProperty(fieldName).ToString();
                if (!groups.ContainsKey(fieldValue))
                {
                    groups.Add(fieldValue, DataObject.NewFileList());
                }
                groups.TryGetValue(fieldValue, out var group);
                group?.Add(file);
            }
            return new DataObject(groups);
        }
    }
}
