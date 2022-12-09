using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    internal class MemoryPropertyTree
    {

        public Folder GetTreeForFolder(string path) { return null; }

        public object GetProperty(string path, QueryObject queryObject) { 
            var tree=GetTreeForFolder(path);
            if (tree == null)
                return null;
            //return tree[queryObject];
            return null;
        }
    }
}
