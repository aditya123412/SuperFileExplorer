using Classes.Common;
using System.Collections.Generic;

namespace Classes.Operations
{
    public class Copy : OperationImplementation
    {
        public static readonly string SOURCE = "Source";
        public static readonly string DEST = "Dest";
        static OperationDescriptor descriptor = new OperationDescriptor("Copy",
            new Dictionary<string, DataObjectType> { { SOURCE, DataObjectType.String }, { DEST, DataObjectType.String } },
            DataObjectType.String);
        static Copy()
        {
            Register(new Copy());
        }
        public Copy()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var source = OperationExecutor.Evaluate(parameters[SOURCE]).GetString();
            var destination = OperationExecutor.Evaluate(parameters[DEST]).GetString();
            System.IO.File.Copy(source,destination);
            return new DataObject(true);
        }
    }
}
