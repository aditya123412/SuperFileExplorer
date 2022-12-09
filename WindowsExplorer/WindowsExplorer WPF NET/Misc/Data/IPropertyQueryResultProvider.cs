using System.Collections.Generic;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    public abstract class IPropertyQueryResultProvider
    {
        public abstract object GetProperty(string path, QueryObject queryObject);
        public List<FieldName> SupportedFields { get; set; } = new List<FieldName>();
    }
}