using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes.Common;

namespace Classes.Operations
{
    internal class OperationStack
    {
        public IList<IDescriptorNode> currentContext;
        public Dictionary<string, IDescriptorNode> currentContextDictionary;
    }
}
