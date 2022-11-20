using System.Text.Json;
using System.Text.RegularExpressions;
using TreeDb.Classes.Services;

namespace TreeDb
{
    [Serializable]
    public class TypeFileRecord
    {
        public List<string> Tags { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
        public FullNodeView Parent { get; set; }
        public string Key { get; set; }

        public TypeFileRecord()
        {
            Tags = new List<string>();
            Attributes = new Dictionary<string, object>();
            Parent = null;
        }

        public TypeFileRecord(Dictionary<string, object>? attributes = null, List<string>? tags = null, FullNodeView parent = null)
        {
            Tags = tags ?? new List<string>();
            Attributes = attributes ?? new Dictionary<string, object>();
            Parent = parent;
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        public void SetTag(string tag)
        {
            Tags.Add(tag);
            Parent?.InvalidateTable();
        }
        public void SetAttribute(string name, string value)
        {
            Attributes[name] = value;
            Parent?.InvalidateTable();
        }
        public bool HasTagIgnoreCase(string tag)
        {
            return Tags.Any(_tag => tag.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
        }
        public bool MatchTag(string tag)
        {
            Regex regex = new Regex(tag);
            return Tags.Any(x => regex.IsMatch(x));
        }

        public object GetPropertyOrDefault(string propertyName, bool createIfNotExist = false, Object defaultValue = default)
        {
            if (!Attributes.ContainsKey(propertyName))
            {
                if (createIfNotExist)
                {
                    Attributes.Add(propertyName, defaultValue);
                }
                return defaultValue;
            }
            return Attributes[propertyName];
        }
        public object this[string index]
        {
            get
            {
                return GetPropertyOrDefault(index);
            }
            set
            {
                Attributes.Add(index, value);
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public void Load(int levels = 0) { }
        public void Unload(bool recursive) { }
    }
}