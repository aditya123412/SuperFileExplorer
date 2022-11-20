using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace TreeDb.Classes
{
    [Serializable]
    public class DatabaseInitConfig
    {
        public string DatabaseName { get; set; }
        public string SaveLocation { get; set; }
        public IEnumerable<string> WatchLocations { get; set; } = new List<string>();
        public IEnumerable<(string name, string path)> RootDirectories { get; set; } = new List<(string name, string path)>();


        public static DatabaseInitConfig LoadFromFile(string saveLocation)
        {
            var fileData = System.IO.File.ReadAllText(saveLocation);

            return JsonSerializer.Deserialize<DatabaseInitConfig>(fileData);
        }
    }
}
