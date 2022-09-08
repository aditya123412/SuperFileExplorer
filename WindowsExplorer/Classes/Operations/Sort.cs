using Classes.Common;

namespace Classes.Operations
{
    public class Sort : OperationImplementation
    {
        public static readonly string Files = "Files";
        public static readonly string Field = "Field";
        public static readonly string Order = "Order";
        static OperationDescriptor descriptor = new OperationDescriptor("Sort",
            new Dictionary<string, DataObjectType> { { Files, DataObjectType.String } },
            DataObjectType.DescriptorNodeList);

        static Sort()
        {
            Register(new Sort());
        }
        public Sort()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var FilesNFolders = OperationExecutor.Evaluate(parameters[Files]).GetDescriptorNodes();
            FilesNFolders.Sort(new Comparer(OperationExecutor.Evaluate(parameters[Field]), OperationExecutor.Evaluate(parameters[Order])));
            return new DataObject(FilesNFolders); ;
        }
    }
    class Comparer : IComparer<IDescriptorNode>
    {
        private DataObject _field;
        private int _isDescending;
        public Comparer(DataObject fieldName, DataObject? order)
        {
            _field = fieldName;
            _isDescending = order?.GetString().Contains("desc", StringComparison.InvariantCultureIgnoreCase) ?? false ? -1 : 1;
        }
        int IComparer<IDescriptorNode>.Compare(IDescriptorNode? x, IDescriptorNode? y)
        {
            switch (x.GetProperty(_field.GetString()).Type)
            {
                case DataObjectType.DescriptorNodeList:
                    return _isDescending * 1;
                case DataObjectType.OperationResultList:
                    return _isDescending * 1;
                case DataObjectType.FileGroupsList:
                    return _isDescending * 1;
                case DataObjectType.StringList:
                    return _isDescending * 1;
                case DataObjectType.String:
                    return _isDescending * x.GetProperty(_field.GetString()).GetString().CompareTo(y.GetProperty(_field.GetString()).ToString());
                case DataObjectType.Number:
                    return _isDescending * (int)(x.GetProperty(_field.GetString()).GetNumber() - y.GetProperty(_field.GetString()).GetNumber());
                case DataObjectType.Boolean:
                    return _isDescending * (x.GetProperty(_field.GetString()).GetBoolean() ? 1 : 0) - (y.GetProperty(_field.GetString()).GetBoolean() ? 1 : 0);
                case DataObjectType.DateTime:
                    return _isDescending * ((int)(x.GetProperty(_field.GetString()).GetDateTime() - y.GetProperty(_field.GetString()).GetDateTime()).TotalMilliseconds);
                case DataObjectType.none:
                    return 1;
                case DataObjectType.SingleFile:
                    return 1;
                case DataObjectType.Expression:
                    return 1;
                case DataObjectType.BLOB:
                    return 1;
                default:
                    return 1;
            }
            ;
        }
    }
}
