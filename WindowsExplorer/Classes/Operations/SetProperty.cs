using Classes.Common;

namespace Classes.Operations
{
    public class SetProperty : OperationImplementation
    {
        public static readonly string File = "File";
        public static readonly string Property = "Property";
        public static readonly string PropValue = "Value";

        static OperationDescriptor descriptor = new OperationDescriptor("SetProperty",
            new Dictionary<string, DataObjectType> {
                {File,DataObjectType.SingleFile },
                {Property, DataObjectType.String },
                {PropValue, DataObjectType.String}
            },
            DataObjectType.Boolean);

        static SetProperty()
        {
            Register(new SetProperty());
        }
        public SetProperty()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var descriptor = OperationExecutor.Evaluate(parameters[File]).GetSingleFile();
            var property = OperationExecutor.Evaluate(parameters[Property]).GetString();
            var value = OperationExecutor.Evaluate(parameters[PropValue]).GetString();
            switch (property.ToLower())
            {
                default:
                    descriptor.SetProperty(property, value);
                    return new DataObject(true); 
                    break;
            }
        }
    }
}
