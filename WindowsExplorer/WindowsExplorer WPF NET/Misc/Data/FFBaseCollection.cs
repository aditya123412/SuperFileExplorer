using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace WindowsExplorer_WPF.Misc
{
    public static class FFBaseCollection
    {
        public static Dictionary<Tkey, ObservableCollection<FFBase>> GroupBy<Tkey>(this ObservableCollection<FFBase> list, Func<FFBase, Tkey> GetGroupName)
        {
            Dictionary<Tkey, ObservableCollection<FFBase>> result = new Dictionary<Tkey, ObservableCollection<FFBase>>();
            foreach (var f in list)
            {
                var groupName = GetGroupName(f);
                if (!result.ContainsKey(groupName))
                {
                    result.Add(GetGroupName(f), new ObservableCollection<FFBase>());
                }
                result[groupName].Add(f);
            };
            return result;
        }

        public static Dictionary<Tkey, ObservableCollection<FFBase>> GroupBy<Tkey>(this ObservableCollection<FFBase> list, Func<FFBase, Tkey> GetGroupName, IEnumerable<Tkey> groupNames)
        {
            Dictionary<Tkey, ObservableCollection<FFBase>> result = new Dictionary<Tkey, ObservableCollection<FFBase>>();
            foreach (var groupName in groupNames)
            {
                result.Add(groupName, new ObservableCollection<FFBase>(new List<FFBase>()));
            }
            foreach (var item in list)
            {
                var groupName = GetGroupName(item);
                result[groupName].Add(item);
            }
            return result;
        }
    }
}
