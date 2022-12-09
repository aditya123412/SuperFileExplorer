using Windows_Explorer.FileAndFolder;

namespace Windows_Explorer.Misc
{
    public static class Operations
    {

        public static List<FFBase> Filter(List<FFBase> items, Func<FFBase, bool> FilterPredicate)
        {
            return items.Where(FilterPredicate).ToList();
        }

        public static List<FFBase> Sort(List<FFBase> items, Func<FFBase, FFBase, int> comparison)
        {
            Comparison<FFBase> comparison1 = new Comparison<FFBase>(comparison);
            var comparer = Comparer<FFBase>.Create(comparison1);
            items.Sort(comparer);
            return items;
        }

        public static IComparer<FFBase> GetComparer()
        {
            Comparison<FFBase> comparison = (FFBase x, FFBase y) => { return 0; };
            return Comparer<FFBase>.Create(comparison);
        }
    }

}
