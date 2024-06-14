using System.Text.Json;
using TreeDb;
using TreeDb.Classes;
using TreeDb.Classes.Persistence;
using TreeDb.Classes.Services;

namespace TreeDb
{
    public class Database
    {
        public string mainSourceLocation;
        public string viewsLocation;
        public string DatabaseName { get; set; }

        ViewFetchingService viewFetchingService { get; set; }
        MemoryTreeService treeService { get; set; }
        RecordsService recordsService { get; set; }

        Logger logger { get; set; }
        public Database(string databaseName, string saveLocation)
        {
            viewsLocation = Path.Combine(saveLocation, "Views");
            FullNodeView.database = this;
            this.DatabaseName = databaseName;
            this.mainSourceLocation = saveLocation;

            IPersistenceInterface persistenceInterface = new FileSystemPersistence(mainSourceLocation);
            recordsService = new RecordsService();
            viewFetchingService = new ViewFetchingService(mainSourceLocation, persistenceInterface);
            treeService = new MemoryTreeService(viewFetchingService, null, 1000);

            logger = new Logger(Path.Combine(saveLocation, "logs.txt"));
        }

        internal string NameFunc(FullNodeView nodeTable)
        {
            return Guid.NewGuid().ToString();
        }

        public Database(DatabaseInitConfig config)
        {
            FullNodeView.database = this;
            DatabaseName = config.DatabaseName;
            mainSourceLocation = config.SaveLocation;

            IPersistenceInterface persistenceInterface = new FileSystemPersistence(mainSourceLocation);
            recordsService = new RecordsService();
            viewFetchingService = new ViewFetchingService(mainSourceLocation, persistenceInterface);
            treeService = new MemoryTreeService(viewFetchingService, null, 1000);
        }

        public FullNodeView GetView(string query)
        {
            // Also implement caching. Views are snapshots, so we can persist additional info or persist query results as views
            var folderReference = GetReferenceFromQuery(query);

            var view = treeService.Navigate(query) as FullNodeView;
            return view;
        }
        public static string GetReferenceFromQuery(string query)
        {
            return QueryService.NormalizeQueryToViewReference(query);
        }
        public FullNodeView GetFromReference(string reference)
        {
            return treeService.GetFromReference(reference, null);
        }
        public FullNodeView CreateOrInitView(string query, bool saveIfNotExist)    //Also gets
        {
            treeService.CreateView(query);
            var view = treeService.Navigate(query) as FullNodeView;
            return view;
        }
        public Dictionary<string, FullNodeView> ProjectView(string query)   // Get all Props records for files in this folder, or Child Records in this view
        {
            var view = GetView(query);
            if (view == null)
                return new Dictionary<string, FullNodeView>();
            else
            {
                var tview = new Dictionary<string, FullNodeView>();
                foreach (var item in view.TableReferences)
                {
                    tview.Add(item.Key, item.Value.Props as FullNodeView);
                }
                return tview;
            }
        }

        public void SaveNode(FullNodeView nodeView, string reference)
        {
            if (nodeView == null)
            {
                viewFetchingService.SaveView(nodeView, reference);
            }
        }
        public Dictionary<string, object> GetAttributes(string query)
        {
            //The attributes of a particular file/folder will actually be stored in the parent view that contains it
            throw new NotImplementedException();
        }

        public void SaveDatabaseToDisk(string filePath)
        {
            var saveData = new DatabaseInitConfig();
            saveData.SaveLocation = mainSourceLocation;
            saveData.DatabaseName = DatabaseName;

            File.WriteAllText(filePath, JsonSerializer.Serialize(saveData));
        }

        //public NodeTable RootTable(string tableName, bool create = false)
        //{
        //    if (!RootDirectories.ContainsKey(tableName))
        //    {
        //        if (!create)
        //        {
        //            return null;
        //        }
        //        RootDirectories.Add(tableName, new NodeTable(tableName, null, null));
        //    } 
        //    return RootDirectories[tableName];
        //}

        public string NormalizeQuery(string query)
        {
            return query;
        }
    }
}