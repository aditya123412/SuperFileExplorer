using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Common
{
    public class FolderProperties
    {
        FileProperties Properties { get; set; }
        IEnumerable<FileProperties> FileProperties { get; set; }
        string orderBy { get; set; }
        string groupBy { get; set; }
        string filterExpression { get; set; }
    }
}
