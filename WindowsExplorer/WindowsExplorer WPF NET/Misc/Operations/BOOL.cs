using Classes.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Operations
{
    public class BOOL : OperationImplementation
    {
        static OperationDescriptor descriptor = new OperationDescriptor("BOOL",
            new Dictionary<string, DataObjectType> { },
            DataObjectType.Boolean);
        static BOOL()
        {
            Register(new BOOL());
        }
        public BOOL()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            if (parameters.Count == 1 && parameters.First().Value == null)
            {
                return new DataObject(parameters.First().Key.Equals("True", StringComparison.InvariantCultureIgnoreCase) || parameters.First().Key.ToString().Equals("1"));
            }
            else
            {
                return new DataObject("Error", DataObjectType.String, parameters.First().Value);
            }
        }
    }
}
