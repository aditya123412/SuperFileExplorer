using System.Text.Json;
using TreeDb;

namespace TreeDb
{
    public class Database
    {
        Dictionary<string, NodeTable> RootDirectories;
        public Database()
        {
            RootDirectories = new Dictionary<string, NodeTable>();
        }
        public NodeTable RootTable(string tableName, bool create = false)
        {
            if (!RootDirectories.ContainsKey(tableName))
            {
                if (!create)
                {
                    return null;
                }
                RootDirectories.Add(tableName, new NodeTable(tableName, null));
            }
            return RootDirectories[tableName];
        }
    }
}