using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDb.Classes.Persistence
{
    public abstract class IPersistenceInterface
    {
        public string Location { get; set; }
        public abstract List<QueryReferenceMapping> GetIndexes(string indexFileName);
        public abstract void SaveIndexes(List<QueryReferenceMapping> indices, string indexFileName);
        public abstract FullNodeView GetFullNodeViewFromReference(string reference);
        public abstract void SaveNodeViewAtReference(FullNodeView newNode, string reference);
        public abstract void DeleteNodeViewReference(string reference);
        public abstract string GetSavePath(FullNodeView fullNodeView);
        public abstract string GetSaveName(FullNodeView fullNodeView);
        public abstract bool CheckExists(string reference);
    }
}
