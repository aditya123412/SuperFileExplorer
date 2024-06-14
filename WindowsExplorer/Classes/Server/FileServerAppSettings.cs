using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Server
{
    public class FileServerAppSettings
    {
        public String StartDirectory { get; set; }
        public String SnapshotsDirectory { get; set; }
        public String SnapshotExtension { get; set; }
        public String ScriptExtension { get; set; }
        public String TagsDirectory { get; set; }
    }
}
