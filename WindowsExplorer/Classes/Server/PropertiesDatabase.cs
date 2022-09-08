using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Server
{
    public static class DatabaseOperations
    {
        public static string NormalizePath(this string path) { return path.ToLower().Replace(" ", ""); }
        public static string Combine(this string path) { return path.ToLower().Replace(" ", ""); }
        public static string Split(this string path) { return path.ToLower().Replace(" ", ""); }
    }
    public class PropertiesDatabase
    {
        List<string> propertyFiles = new List<string>();
        public void Initialize(string propertiesDirectory)
        {
            System.IO.DirectoryInfo propertiesDirectoryInfo = new System.IO.DirectoryInfo(propertiesDirectory);
            propertiesDirectoryInfo.EnumerateFiles();
        }

        public string[] GetPath(string path) { return path.Normalize().Split('\\'); }
    }
    public class DatabaseRecord<ValueType>
    {
        public Dictionary<string, DatabaseRecord<ValueType>> records = new Dictionary<string, DatabaseRecord<ValueType>>();
        public ValueType value { get; set; }
        public string name { get; set; }

        private DatabaseRecord<ValueType> parent;
        public DatabaseRecord(string name, ValueType value, DatabaseRecord<ValueType> parent = null)
        {
            this.name = name;
            this.value = value;
            this.parent = parent;
        }
        public DatabaseRecord<ValueType> GetParent() { return parent; }
        public DatabaseRecord<ValueType> GetRecord(string name)
        {
            return records.GetValueOrDefault(name);
        }
        public IEnumerable<DatabaseRecord<ValueType>> GetRecords(Func<DatabaseRecord<ValueType>, bool> predicate)
        {
            return records.Values.Where(predicate);
        }

        public bool DeleteRecord(string name)
        {
            try
            {
                records.Remove(name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void AddRecord(DatabaseRecord<ValueType> value)
        {
            value.parent = this;
            records.Add(name, value);
        }
        public void DeleteRecords(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                DeleteRecord(name);
            }
        }

        public void Move(DatabaseRecord<ValueType> Dest, string name)
        {
            this.name = name;
            Dest.AddRecord(this);
        }
        public void Copy(DatabaseRecord<ValueType> Dest, string name)
        {
            var record = new DatabaseRecord<ValueType>(name, this.value, Dest);
            Dest.AddRecord(record);
        }
    }

}
