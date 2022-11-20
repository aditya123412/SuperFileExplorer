using System.Collections.Generic;
using Windows_Explorer.FileAndFolder;
using Windows_Explorer.Misc;
using System.Linq;

namespace Windows_Explorer.Misc
{
    public class FFBaseCollection : List<FFBase>
    {

        //Events:==
        public Action<FFBaseCollection> OnItemsChange = null;
        public Action<FFBaseCollection> OnAdd = null;
        public Action<FFBaseCollection> OnRemoveItem = null;
        public Action<FFBaseCollection> OnOrderChange = null;
        public Action<FFBase> OnItemChange = null;


        public new FFBase this[int index]
        {
            get
            {
                return base[index];
            }

            set
            {
                base[index] = value;
                OnItemsChange(this);
            }
        }
        public new void Add(FFBase fFBase)
        {
            base.Add(fFBase);
            //OnAdd(new FFBaseCollection() { fFBase });
            //OnItemsChange(this);
        }
        public new void AddRange(IEnumerable<FFBase> fFBases)
        {
            base.AddRange(fFBases);
            //OnAdd(new FFBaseCollection(fFBases) );
            //OnItemsChange(this);
        }
        public new bool Remove(FFBase item)
        {
            OnRemoveItem(new FFBaseCollection() { item});
            var result = base.Remove(item);
            OnItemsChange(this);
            return result;
        }
        public new void RemoveAt(int index)
        {
            var item = base[index];
            OnRemoveItem(new FFBaseCollection() { item });
            base.Remove(item);
            OnItemsChange(this);
        }
        public new void RemoveRange(int start, int count)
        {
            base.RemoveRange(start, count);
            OnItemsChange(this);
        }
        public void Sort<FFBase>(IComparer<FileAndFolder.FFBase>? comparer)
        {
            base.Sort(comparer);
            OnOrderChange(this);
        }
        public void Sort<Tkey>(Func<FFBase, Tkey> FunctionGetter, Func<Tkey, Tkey, int>? comparer)
        {
            Comparison<FFBase> comparison = new Comparison<FFBase>((f1, f2) => comparer(FunctionGetter(f1), FunctionGetter(f2)));
            var comparerFinal = Comparer<FFBase>.Create(comparison);
            base.Sort(comparerFinal);
            OnOrderChange(this);
        }

        public Dictionary<Tkey, FFBaseCollection> GroupBy<Tkey>(Func<FFBase, Tkey> GetGroupName)
        {
            Dictionary<Tkey, FFBaseCollection> result = new Dictionary<Tkey, FFBaseCollection>();
            this.ForEach(f =>
            {
                var groupName = GetGroupName(f);
                if (!result.ContainsKey(groupName))
                {
                    result.Add(GetGroupName(f), new FFBaseCollection(new List<FFBase>()));
                }
                result[groupName].Add(f);
            });
            return result;
        }

        public Dictionary<Tkey, FFBaseCollection> GroupBy<Tkey>(Func<FFBase, Tkey> GetGroupName, IEnumerable<Tkey> groupNames)
        {
            Dictionary<Tkey, FFBaseCollection> result = new Dictionary<Tkey, FFBaseCollection>();
            foreach (var groupName in groupNames)
            {
                result.Add(groupName, new FFBaseCollection(new List<FFBase>()));
            }
            this.ForEach(f =>
            {
                var groupName = GetGroupName(f);
                result[groupName].Add(f);
            });
            return result;
        }

        public FFBaseCollection(List<FFBase> items) : base(items)
        {

        }
        public FFBaseCollection() : base(new List<FFBase>())
        {

        }
        public FFBaseCollection(IEnumerable<FFBase> collection) : base(collection)
        {
        }
    }
}
