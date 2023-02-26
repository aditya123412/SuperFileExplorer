using Classes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Operations
{
    public abstract class OperationImplementation
    {
        public OperationDescriptor Descriptor { get; set; }
        //public abstract DataObject Execute(Dictionary<string, DataObject> parameters);
        public abstract DataObject Evaluate(Dictionary<string, OperationExpression> parameters);
        public List<FileItem> Context;
        public object EvaluateNative() { return null; }
        static public void Register(OperationImplementation implementation)
        {
            OperationExecutor.RegisterOperationDescription(implementation.Descriptor);
            OperationExecutor.RegisterOperationImplementation(implementation);
        }
        public void Register1()
        {
        }
    }
}
