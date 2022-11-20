using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDb.Classes.Services
{
    public class RecordsService
    {
        public TypeFileRecord GetFileRecordByKey(string key)
        {
            throw new NotImplementedException();
        }
        public TypeFileRecord GetFileRecordByQuery(string query)
        {
            throw new NotImplementedException();
        }

        public TypeFileRecord GetFileRecordsByKeys(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public void SaveFileRecord(TypeFileRecord record, string key) { }
    }
}
