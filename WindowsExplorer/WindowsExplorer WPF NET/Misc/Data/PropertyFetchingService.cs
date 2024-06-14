using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    internal class PropertyFetchingService
    {
        public Object FetchProperty(string path, IEnumerable<QueryObject> PropertyFieldsForQuery, IEnumerable<IPropertyQueryResultProvider> PropertyQueryResultProviders, Func<Folder> TreeBranchFilter, Func<object, object> Transformation = null)
        {
            Dictionary<QueryObject, object> result = new Dictionary<QueryObject, object>();
            foreach (var propertyFieldForQuery in PropertyFieldsForQuery)
            {
                foreach (var propertyQueryResultProvider in PropertyQueryResultProviders)
                {
                    object property = propertyQueryResultProvider.GetProperty(path, propertyFieldForQuery);
                    if (property != null)
                    {
                        result.Add(propertyFieldForQuery, property);
                    }
                }
            }
            return result;
        }
    }
}
