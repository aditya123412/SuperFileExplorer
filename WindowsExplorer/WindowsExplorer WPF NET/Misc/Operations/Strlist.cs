using Classes.Common;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Operations
{
    public class StrList : OperationImplementation
    {
        static readonly string STRLIST = "StrList";
        static OperationDescriptor descriptor = new OperationDescriptor(STRLIST,
            new Dictionary<string, DataObjectType> { },
            DataObjectType.Boolean);
        static StrList()
        {
            Register(new StrList());
        }
        public StrList()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            return new DataObject(parameters.Keys.ToList());
        }
    }
}
