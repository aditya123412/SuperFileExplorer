using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF_NET.Misc
{
    public class TreeNodeItem : INotifyPropertyChanged
    {
        public string Caption { get; set; }
        public string FullPath { get; set; }
        public bool IsExpanded { get; set; }
        public FFBase fFBase { get; set; }
        public BitmapSource Thumbnail { get; set; }
        public Action<TreeNodeItem> Click { get; set; } = (TreeNodeItem t) => { };
        public ObservableCollection<TreeNodeItem> TreeData { get; set; }

        public TreeNodeItem(string caption, string fullpath, bool createDefaultCollectionChild = false, bool expand = false)
        {
            TreeData = createDefaultCollectionChild ? new ObservableCollection<TreeNodeItem> { new TreeNodeItem("", "") } : new ObservableCollection<TreeNodeItem>();
            Caption = caption;
            FullPath = fullpath;
            IsExpanded = expand;
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

        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}
