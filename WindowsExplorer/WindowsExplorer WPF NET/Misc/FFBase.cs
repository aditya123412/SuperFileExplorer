using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Timers;
using System.Windows.Controls;
using System.ComponentModel;
using WindowsExplorer_WPF_NET.Misc.Data;
using WindowsExplorer_WPF_NET.Controls;
using System.Windows;
using System.Collections.ObjectModel;

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
        public int X { get; set; }
        public int Y { get; set; }

        public DateTime LastModified { get; set; }
        public DateTime LastAccessed { get; set; }
        public DateTime Created { get; set; }
        public bool SupportsThumbnail { get; set; }
        public Visibility MenuVisible { get; set; } = Visibility.Collapsed;
        public void MouseDownAction(object sender, System.Windows.Input.MouseButtonEventArgs e, int X = 0, int Y = 0)
        {
            switch (e.ChangedButton)
            {
                case System.Windows.Input.MouseButton.Left:
                    SingleClickIcon();
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
                    break;
                case System.Windows.Input.MouseButton.Middle:
                    break;
                case System.Windows.Input.MouseButton.Right:
                    var commandsMenu = new Commands_Menu();
                    commandsMenu.Show();
                    commandsMenu.Top = Y - commandsMenu.Height / 2;
                    commandsMenu.Left = X - commandsMenu.Width / 2;
                    break;
                case System.Windows.Input.MouseButton.XButton1:
                    break;
                case System.Windows.Input.MouseButton.XButton2:
                    break;
                default:
                    break;
            }
        }

        public Action DoubleClickIcon = () => { };
        public Action SingleClickIcon = () => { };

        public event PropertyChangedEventHandler PropertyChanged = (o, s) => { };

        public FFBase()
        {

            timer = new Timer();
            timer.Interval = 200;
            timer.Elapsed += new ElapsedEventHandler(Elapsed_EventHandler);
        }
        public FFBase(FFBase baseObj, ICollection<QueryObject> queryObjects, ICollection<IPropertyQueryResultProvider> PropertyQueryResultProviders)
        {
            var propService = new PropertyFetchingService();
            foreach (var queryObject in queryObjects)
            {

                var prop = propService.FetchProperty(baseObj.FullPath, queryObjects, PropertyQueryResultProviders, null);
                if (prop != null)
                    SetProperty(queryObject, prop);
            }
        }

        public object this[FieldName fieldName]
        {
            get
            {
                switch (fieldName)
                {
                    case FieldName.Name:
                        return Name;
                    case FieldName.Created:
                        return Created;
                    case FieldName.Extension:
                        return (this as File).Extension;
                    case FieldName.DefaultIcon:
                        return DefaultIcon;
                    case FieldName.Thumbnail:
                        return Thumbnail;
                    case FieldName.Size:
                        return Size;
                    case FieldName.FullPath:
                        return FullPath;
                    case FieldName.Location:
                        return Location;
                    case FieldName.IsFolder:
                        return IsFolder;
                    case FieldName.ThumbnailLoaded:
                        break;
                    case FieldName.SupportsThumbnail:
                        break;
                    case FieldName.LastModified:
                        return LastModified;
                    case FieldName.LastAccessed:
                        return LastAccessed;
                    case FieldName.LastUpdatedBy:
                        break;
                    case FieldName.LastAccessedBy:
                        break;
                    case FieldName.Type:
                        break;
                    default:
                        break;
                }
                return null;
            }
            set
            {
                switch (fieldName)
                {
                    case FieldName.Name:
                        Name = value as string;
                        break;
                    case FieldName.Created:
                        Created = (DateTime)value;
                        break;
                    case FieldName.Extension:
                        (this as File).Extension = value as string;
                        break;
                    case FieldName.DefaultIcon:
                        DefaultIcon = value as BitmapImage;
                        break;
                    case FieldName.Thumbnail:
                        Thumbnail = value as BitmapImage;
                        HasThumbnail = true;
                        break;
                    case FieldName.Size:
                        Size = (int)value;
                        break;
                    case FieldName.FullPath:
                        FullPath = (string)value;
                        break;
                    case FieldName.Location:
                        Location = (string)value;
                        break;
                    case FieldName.IsFolder:
                        IsFolder = (bool)value;
                        break;
                    case FieldName.SupportsThumbnail:
                        SupportsThumbnail = (bool)value;
                        break;
                    case FieldName.LastModified:
                        LastModified = (DateTime)value;
                        break;
                    case FieldName.LastAccessed:
                        LastAccessed = (DateTime)value;
                        break;
                    case FieldName.Type:
                        Type = (Type)value;
                        break;
                    default:
                        break;
                }
            }
        }
        private void SetProperty(QueryObject queryObject, object prop)
        {
            switch (queryObject.FieldName)
            {
                case FieldName.Name:
                    break;
                case FieldName.Created:
                    break;
                case FieldName.Extension:
                    break;
                case FieldName.DefaultIcon:
                    break;
                case FieldName.Thumbnail:
                    break;
                case FieldName.Size:
                    break;
                case FieldName.FullPath:
                    break;
                case FieldName.Location:
                    break;
                case FieldName.IsFolder:
                    break;
                case FieldName.ThumbnailLoaded:
                    break;
                case FieldName.SupportsThumbnail:
                    break;
                case FieldName.LastModified:
                    break;
                case FieldName.LastAccessed:
                    break;
                case FieldName.LastUpdatedBy:
                    break;
                case FieldName.LastAccessedBy:
                    break;
                case FieldName.Type:
                    break;
                default:
                    break;
            }
        }

        private object GetProperty(QueryObject queryObject)
        {
            switch (queryObject.FieldName)
            {
                case FieldName.Name:
                    return Name;
                case FieldName.Created:
                    return Created;
                case FieldName.Extension:
                    return (this as File).Extension;
                case FieldName.DefaultIcon:
                    return DefaultIcon;
                case FieldName.Thumbnail:
                    return Thumbnail;
                case FieldName.Size:
                    return Size;
                case FieldName.FullPath:
                    return FullPath;
                case FieldName.Location:
                    return Location;
                case FieldName.IsFolder:
                    return IsFolder;
                case FieldName.ThumbnailLoaded:
                    break;
                case FieldName.SupportsThumbnail:
                    break;
                case FieldName.LastModified:
                    return LastModified;
                case FieldName.LastAccessed:
                    return LastAccessed;
                case FieldName.LastUpdatedBy:
                    break;
                case FieldName.LastAccessedBy:
                    break;
                case FieldName.Type:
                    break;
                default:
                    break;
            }
            return null;
        }

        private void Elapsed_EventHandler(object sender, ElapsedEventArgs e)
        {
            isSecondClick = false;
            timer.Stop();
        }
    }

    public enum Type { File, Folder, Any, CustomScript }
}
