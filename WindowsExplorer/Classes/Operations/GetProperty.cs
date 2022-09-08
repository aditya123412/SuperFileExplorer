using Classes.Common;

namespace Classes.Operations
{
    public class GetProperty : OperationImplementation
    {
        public static readonly string File = "File";
        public static readonly string PropertyName = "Property";
        static OperationDescriptor descriptor = new OperationDescriptor("GetProperty",
            new Dictionary<string, DataObjectType> { { File, DataObjectType.SingleFile }, { PropertyName, DataObjectType.String } },
            DataObjectType.String);
        static GetProperty()
        {
            Register(new GetProperty());
        }
        public GetProperty()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var fileObject = OperationExecutor.Evaluate(parameters[File]);
            var propertyName = OperationExecutor.Evaluate(parameters[PropertyName]).GetString();
            return fileObject.GetSingleFile().GetProperty(propertyName);
        }
    }
}
