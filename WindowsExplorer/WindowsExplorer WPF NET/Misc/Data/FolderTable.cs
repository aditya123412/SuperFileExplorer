using System.Collections.Generic;
using WindowsExplorer_WPF.Misc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    public class FolderTable
    {
        public Dictionary<string, FFBase> _filedata { get; set; }

        public FolderTable()
        {
            _filedata = new Dictionary<string, FFBase>();
        }
        public FolderTable(Dictionary<string, FFBase> filedata) { _filedata = filedata; }

        public FFBase this[string fileName]
        {
            get
            {
                return _filedata[fileName];
            }
        }
    }
}