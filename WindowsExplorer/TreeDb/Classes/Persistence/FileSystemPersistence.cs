using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace TreeDb.Classes.Persistence
{
    public class FileSystemPersistence : IPersistenceInterface
    {
        public FileSystemPersistence(string location)
        {
            Location = location;
        }

        public override FullNodeView GetFullNodeViewFromReference(string reference)
        {
            var viewData = File.ReadAllText(Path.Combine(Location, reference));
            return JsonSerializer.Deserialize<SerializableTableData>(viewData).CreateNodeTable();
        }

        public override void SaveNodeViewAtReference(FullNodeView newNode, string reference)
        {
            File.WriteAllText(Path.Combine(Location, reference), JsonSerializer.Serialize(SerializableTableData.CreateFromTable(newNode)));
        }

        public override void DeleteNodeViewReference(string reference)
        {
            System.IO.File.Delete(Path.Combine(Location, reference));
        }

        public override bool CheckExists(string reference)
        {
            return File.Exists(Path.Combine(Location, reference));
        }
        public override List<QueryReferenceMapping> GetIndexes(string reference)
        {
            var viewData = File.ReadAllText(reference);
            return JsonSerializer.Deserialize<List<QueryReferenceMapping>>(viewData);
        }
        public override void SaveIndexes(List<QueryReferenceMapping> indices, string indexFileName)
        {
            File.WriteAllText(Path.Combine(Location, indexFileName), JsonSerializer.Serialize(indices));
        }

        public override string GetSaveName(FullNodeView fullNodeView)
        {
            return new Guid().ToString();
        }

        public override string GetSavePath(FullNodeView fullNodeView)
        {
            return Path.Combine(Location, fullNodeView.SaveName);
        }

    }
}
