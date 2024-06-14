using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Operations
{
    public class OperationExpression
    {
        public string OperationName { get; set; }
        public Dictionary<string, OperationExpression> Arguments { get; set; }
        public DataObject evaluate()
        {
            return OperationExecutor.Evaluate(this);
        }
    }
}
