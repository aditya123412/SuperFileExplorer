using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    public class DbDirectory : IPropertyQueryResultProvider
    {
        FolderTable dataTable;
        public DbDirectory(FolderTable DataTable) : this()
        {
            dataTable = DataTable;
        }
        private DbDirectory()
        {
            SupportedFields = new List<FieldName> {
                FieldName.Size, FieldName.Location, FieldName.Extension, FieldName.SupportsThumbnail,
                FieldName.IsFolder, FieldName.LastAccessed, FieldName.LastModified
            };
        }
        public override object GetProperty(string path, QueryObject queryObject)
        {
            var fname = Path.GetFileName(path);
            FFBase item = dataTable[fname];
            if (item == null)
                return null;
            if (SupportedFields.Contains(queryObject.FieldName))
            {
                switch (queryObject.FieldName)
                {
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}
