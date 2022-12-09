using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    internal class FileOperationsService
    {
        public long GetSize(Folder fFBase)
        {
            var totalSize = 0l;
            foreach (var item in fFBase.Items)
            {
                if (item.Type == WindowsExplorer_WPF.Misc.Type.Folder)
                {
                    totalSize += GetSize(item as Folder);
                }
                else
                {
                    totalSize += (item as File).Size;
                }
            }
            return totalSize;
        }
    }
}
