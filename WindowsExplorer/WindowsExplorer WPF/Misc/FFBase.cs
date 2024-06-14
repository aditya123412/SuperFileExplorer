using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Timers;
using System.ComponentModel;

namespace WindowsExplorer_WPF.Misc
{
    public class FFBase : System.ComponentModel.INotifyPropertyChanged
    {
        private Timer timer;
        bool isSecondClick = false;

        public const string Click = "Click";
        public const string DoubleClick = "DoubleClick";
        public const string CopyToClipboard = "CopyToClipboard";
        public const string OnSelect = "OnSelect";
        public const string OnUnSelect = "OnUnSelect";
        public Type Type { get; set; }
        public BitmapImage DefaultIcon { get; set; }
        public BitmapSource Thumbnail { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Location { get; set; }
        public string FullPath { get; set; }
        public bool IsFolder { get; set; }
        public bool HasThumbnail { get; set; }
        public bool Selected { get; set; }

        public DateTime LastModified { get; set; }
        public DateTime LastAccessed { get; set; }
        public DateTime Created { get; set; }
        public bool SupportsThumbnail { get; set; }

        public void MouseDownAction(object sender, object e)
        {
            if (isSecondClick)
            {
                DoubleClickIcon();
                isSecondClick = false;
                timer.Stop();
            }
            else
            {
                isSecondClick = true;
                timer.Start();
            }
        }

        public Action DoubleClickIcon = () => { };

        public event PropertyChangedEventHandler? PropertyChanged = (o, s) => { };

        public FFBase()
        {

            timer = new Timer();
            timer.Interval = 200;
            timer.Elapsed += new ElapsedEventHandler(Elapsed_EventHandler);
        }
        private void Elapsed_EventHandler(object? sender, ElapsedEventArgs e)
        {
            isSecondClick = false;
            timer.Stop();
        }
    }

    public enum Type { File, Folder, Any, CustomScript }
}
