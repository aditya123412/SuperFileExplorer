using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace TreeDb
{
    public class TypeFileRecord
    {
        public IEnumerable<string> Tags;
        public Dictionary<string, object> Attributes;
        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
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
    }
}