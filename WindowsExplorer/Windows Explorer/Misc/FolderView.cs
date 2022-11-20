using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_Explorer.Misc
{
    public class FolderView
    {
        public Image BackGroundImage { get; set; }
        public List<FileAndFolder.FFBase> Items { get; set; }
        public List<FileAndFolder.FFBase> SelectedItems { get; set; }
    }
}
