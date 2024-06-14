using Classes.Common;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Operations
{
    public class STR : OperationImplementation
    {
        static OperationDescriptor descriptor = new OperationDescriptor("STR",
            new Dictionary<string, DataObjectType> { },
            DataObjectType.String);
        static STR()
        {
            Register(new STR());
        }
        public STR()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            if (parameters.Count == 1 && parameters.First().Value == null)
            {
                return new DataObject(parameters.First().Key);
            }
            else
            {
                return new DataObject("Error", DataObjectType.String, parameters.First().Value);
            }
        }
    }
}
