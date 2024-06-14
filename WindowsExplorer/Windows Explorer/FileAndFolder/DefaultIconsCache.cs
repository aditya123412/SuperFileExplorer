namespace Windows_Explorer.FileAndFolder
{
    public class DefaultIconsCache
    {
        private Dictionary<string, Icon> _iconsCache = new Dictionary<string, Icon>();

        public Icon GetIcon(string extension, string path)
        {
            if (!_iconsCache.ContainsKey(extension))
            {
                var icon = Icon.ExtractAssociatedIcon(path);
                Update(extension, icon);
            }
            return _iconsCache[extension];
        }
        public void Update(string extension, Icon icon)
        {
            _iconsCache[extension] = icon;
        }
    }
}
