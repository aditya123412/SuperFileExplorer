using System.Text.Json.Serialization;

namespace TreeDb.Classes.MemoryDataStructures
{
    public class TempNodeView: TypeFileRecord
    {
        public Dictionary<string, (string saveName, TypeFileRecord Props)> TableReferences { get; set; }

        [JsonIgnore]
        public static Database database;

        //[JsonIgnore]
        //public Dictionary<string, FullNodeView> RecordTables { get; set; }

        [JsonIgnore]
        public bool AdditionalMetadataLoaded { get; set; }
    }
}
