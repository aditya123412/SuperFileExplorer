using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_Explorer.FileAndFolder;

namespace Windows_Explorer.Misc
{
    public class FolderViewSnapshot
    {
        static private string mainGroupName = "$Main";

        public Dictionary<string, List<FFBase>> Items { get; set; }
        public Dictionary<string, AdditionalGroupProperties> AdditionalGroupProperties { get; set; }
        public DateTime LastUpdated { get; set; }
        public FolderViewSnapshot(List<FFBase> items)
        {
            Items = new Dictionary<string, List<FFBase>>();
            foreach (var item in items)
            {
                //if (!Items.ContainsKey(item.))
                //{

                //}
            }
        }
        public List<string> GetRelevantTags()
        {
            return null;
        }
        public List<string> GetGroupNames()
        {
            return Items.Keys.ToList<string>();
        }
        public List<FFBase> GetAllItems() { return null; }
        public List<FFBase> GetDefaultItems() { return null; }
    }
    public class AdditionalGroupProperties
    {

    }
}
