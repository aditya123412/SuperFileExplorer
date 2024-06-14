using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Common
{
    public class FileProperties
    {
        public IDictionary<string, object> Properties { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
