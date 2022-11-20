using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeDb;
using Windows_Explorer.ActiveControls;
using Windows_Explorer.Misc;

namespace Windows_Explorer.FileAndFolder
{
    public class FFBase : TypeFileRecord
    {
        public const string Click = "Click";
        public const string DoubleClick = "DoubleClick";
        public const string CopyToClipboard = "CopyToClipboard";
        public const string OnSelect = "OnSelect";
        public const string OnUnSelect = "OnUnSelect";
        public Type Type { get; set; }
        public Image DefaultIcon { get; set; }
        public Image Thumbnail { get; set; }
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

        public Func<string, Image> ThumbnailGetter { get; set; } = (string x) => null;
        public Func<string, Image> DefaultIconGetter { get; set; } = (string x) => null;

        public Dictionary<string, Func<FFBase, object>> DataActions { get; set; } = new Dictionary<string, Func<FFBase, object>>
        {
            {Click,(FFBase x) => null },
            {DoubleClick,(FFBase x) => null },
            {CopyToClipboard,(FFBase x) => null },
            {OnSelect,(FFBase x) => null },
            {OnUnSelect,(FFBase x) => null }
        };
        public List<string> ActionsList { get { return DataActions.Keys.ToList(); } }

    }

    public enum Type { File, Folder, Any, CustomScript }
}
