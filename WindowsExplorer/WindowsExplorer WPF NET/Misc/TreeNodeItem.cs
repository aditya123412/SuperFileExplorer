using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WindowsExplorer_WPF_NET.Misc
{
    public class TreeNodeItem
    {
        public string Caption { get; set; }
        public string FullPath { get; set; }
        public bool IsExpanded { get; set; }

        public ObservableCollection<TreeNodeItem> TreeData { get; set; } = new ObservableCollection<TreeNodeItem>() { null };

        public TreeNodeItem(string caption, string fullpath)
        {
            TreeData = new ObservableCollection<TreeNodeItem>();
            Caption = caption;
            FullPath = fullpath;
        }
        public void Expand()
        {
            if (!IsExpanded)
            {
                IsExpanded = true;
            }
            if (TreeData.Count == 0 || TreeData.First() == null)
            {
                var di = new DirectoryInfo(FullPath);
                foreach (var item in di.GetDirectories())
                {
                    TreeData.Add(new TreeNodeItem(item.Name, item.FullName));
                }
            }
        }
    }
}
