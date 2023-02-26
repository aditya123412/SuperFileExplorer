using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Operations
{
    public class OperationDescriptor
    {
        public OperationDescriptor(string name, Dictionary<string, DataObjectType> parameters, DataObjectType output)
        {
            OperationName = name;
            Parameters = parameters;
            OutputType = output;
            OperationExecutor.RegisterOperationDescription(this);
        }
        public string OperationName { get; set; }
        public Dictionary<string, DataObjectType> Parameters { get; set; }
        public DataObjectType OutputType { get; set; }
    }
}
