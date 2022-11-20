namespace TreeDb.Classes
{
    /* 
     * This class will be what is actually saved to the disk.
     * This class will be a proxy for the NodeTable and will be serializable both ways
     * This class will be free of any data that contains nested recursive references
     * 
     * We want to save the attributes of the NodeTable and its immediate children in each object.
     * */
    [Serializable]
    public class SerializableTableData
    {
        public SerializableTableData()
        {
            ChildAttributes = new Dictionary<string, SerializableTableData>();
        }
        public Dictionary<string, object> Attributes { get; set; }
        public List<string> Tags { get; set; }

        public Dictionary<string, SerializableTableData> ChildAttributes { get; set; }

        public List<(string tableName, string storageReference)> ChildStorageNames { get; set; }
        public string Name { get; private set; }

        public static SerializableTableData CreateFromTable(FullNodeView table)
        {
            var serializableData = new SerializableTableData()
            {
                Attributes = table.Attributes,
                Tags = table.Tags,
                ChildStorageNames = table.TableReferences.Select(x =>
                {
                    return (x.Key, (x.Value.Props as FullNodeView).SaveName);
                }).ToList<(string tableName, string storageName)>()
            };
            foreach (var childTable in table.TableReferences)
            {
                serializableData.ChildAttributes[childTable.Key] = new SerializableTableData() { Attributes = childTable.Value.Props.Attributes, Tags = childTable.Value.Props.Tags };
            }

            return serializableData;
        }

        public FullNodeView CreateNodeTable(FullNodeView parent = null)
        {
            var table = new FullNodeView(Name, parent);
            table.Attributes = Attributes;
            table.Tags = Tags;

            foreach (var childTable in ChildStorageNames)
            {
                table.InsertViewRecord(childTable.tableName, new FullNodeView(childTable.tableName, table)
                {
                    Attributes = ChildAttributes[childTable.tableName].Attributes,
                    Tags = ChildAttributes[childTable.tableName].Tags,
                    SaveName = childTable.storageReference
                });
            }
            return table;
        }
    }
}
