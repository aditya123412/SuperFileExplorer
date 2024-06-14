using TreeDb.Classes.Persistence;
using System.Text.Json;

namespace TreeDb.Classes.Services
{
    public class ViewFetchingService
    {
        Dictionary<string, FullNodeView> viewCache;
        public string viewsCacheDirectory { get; set; }
        IPersistenceInterface fileSystem { get; set; }

        public ViewFetchingService(string viewsDirectory, IPersistenceInterface persistenceInterface)
        {
            viewsCacheDirectory = viewsDirectory;
            fileSystem = persistenceInterface;
            viewCache = new Dictionary<string, FullNodeView>();
        }
        public FullNodeView GetView(string reference, FullNodeView parent)
        {
            if (viewCache.ContainsKey(reference))
                return viewCache[reference];

            if (fileSystem.CheckExists(reference))
            {
                var view = fileSystem.GetFullNodeViewFromReference(reference);
                view.Parent = parent;
                viewCache[reference] = view;
                return view;
            }
            return null;
        }
        public void SaveView(FullNodeView view, string reference)
        {
            viewCache[reference] = view;
            fileSystem.SaveNodeViewAtReference(view, reference);
        }


        public bool IfViewExistsForReference(string reference)
        {
            return fileSystem.CheckExists(reference);
        }

        public void RefreshViewRecordsFromMaster(IEnumerable<string> recordKeys)
        {

        }

        public void SaveViewsCache()
        {
            File.WriteAllText(Path.Combine(viewsCacheDirectory, $"{Guid.NewGuid().ToString()}.views"), JsonSerializer.Serialize(viewCache));
        }

        public void LoadViewsCache()
        {
            viewCache = new Dictionary<string, FullNodeView>();
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(viewsCacheDirectory);
            foreach (System.IO.FileInfo file in directoryInfo.GetFiles())
            {
                LoadCacheFromFile(file.FullName);
            }
        }

        private void LoadCacheFromFile(string fullName)
        {
            var _viewCache = JsonSerializer.Deserialize<Dictionary<string, FullNodeView>>(File.ReadAllText(fullName, System.Text.Encoding.UTF8));
            foreach (var kvp in _viewCache.ToList())
            {
                viewCache[kvp.Key] = kvp.Value;
            }
        }

        public void ConsolidateViewsCacheFiles(string viewsCacheDirectory, IEnumerable<string> whiteList, IEnumerable<string> blackList)
        {

        }
    }
}
