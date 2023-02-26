using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WindowsExplorer_WPF_NET.Misc
{
    internal class PropertiesView : INotifyPropertyChanged
    {
        public List<BitmapSource> BitmapSources { get; set; }=new List<BitmapSource>();
        public List<string> OtherProperties { get; set; }   = new List<string>();
        public long TotalSize { get; set; }
        public int SelectedCount { get; set; }
        public bool ShowTagsBox { get; set; }


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
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
    }
}
