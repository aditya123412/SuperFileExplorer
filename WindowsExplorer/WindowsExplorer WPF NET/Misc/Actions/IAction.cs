using System.Windows.Media.Imaging;

namespace WindowsExplorer_WPF_NET.Misc.Actions
{
    internal class IAction
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BitmapImage DefaultIcon { get; set; }
        
    }
}
