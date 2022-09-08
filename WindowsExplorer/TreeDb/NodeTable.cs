using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TreeDb
{
    [Serializable]
    public class NodeTable
    {
        public Dictionary<string, NodeTable> Tables { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<string, object> Attributes { get; set; }

        private readonly string PARENT = "parent";
        private readonly string NAME = "name";
        private readonly string ATTRIBUTE_QUERY = "@";
        private readonly string TAG_QUERY = "?";
        private readonly string PARENT_QUERY = "^";
        private readonly string FOLDER_SEP = @"\";
        private readonly string QUERY_SEP = @"|";
        private readonly string WILD_CARD = @"*";
        private readonly string QUERY_AS_FOLDER = "[]";

        public NodeTable(string name, NodeTable parent)
        {
            Tables = new Dictionary<string, NodeTable>();
            Attributes = new Dictionary<string, object>();
            Tags = new List<string>();
            Attributes.Add(PARENT, parent);
            Attributes.Add(NAME, name);
        }

        public void Init(Func<Dictionary<string, NodeTable>> initFunction, string name, NodeTable parent = null)
        {
            Tables = initFunction();
            Tags = new List<string>();
            Attributes.Add(PARENT, parent);
            Attributes.Add(NAME, name);
        }
        public NodeTable GetParent()
        {
            return (NodeTable)Attributes[PARENT];
        }

        public string Serialize(Func<NodeTable, string> serializationFunction)
        {
            return serializationFunction(this);
        }

        public NodeTable Get(string key)
        {
            return Tables[key];
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }
        public void PutTag(string tag)
        {
            Tags.Add(tag);
        }

        public object GetAttribute(string attributeName)
        {
            if (Attributes.ContainsKey(attributeName))
            {
                return Attributes[attributeName];
            }
            else
            {
                return null;
            }
        }

        public NodeTable GetOrCreateRecord(string key)
        {
            if (!Tables.ContainsKey(key))
            {
                Tables.Add(key, new NodeTable(key, this));
            }
            return Tables[key];
        }

        public void Put(string key, NodeTable fileRecord)
        {
            Tables[key] = fileRecord;
        }

        public IEnumerable<NodeTable> Query(string query, bool createMode = false)
        {
            var path = query.Split(FOLDER_SEP);
            var collection = (IEnumerable<NodeTable>)new List<NodeTable> { this };
            for (int i = 0; i < path.Length; i++)
            {
                var position = path[i];
                collection = collection.Select(x =>
                {
                    return x.GetOrCreateRecord(position);
                });

                return collection;
            }

            throw new NotImplementedException();
        }

        public object this[string key]
        {
            get
            {
                if (key.StartsWith(ATTRIBUTE_QUERY))
                {
                    key = key.Substring(1);
                    return GetAttribute(key);
                }
                if (key.StartsWith(TAG_QUERY))
                {
                    key = key.Substring(1);
                    return HasTag(key);
                }
                if (key.StartsWith(PARENT_QUERY) || key.Equals(PARENT, StringComparison.CurrentCultureIgnoreCase) || key.Equals(".."))
                {
                    return GetParent();
                }
                return GetOrCreateRecord(key);
            }
            set
            {
                if (value is NodeTable)
                {
                    Put(key, (NodeTable)value);
                }
                else if (value is string)
                {
                    Attributes[key] = value;
                }
            }
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true });
        }
    }
}