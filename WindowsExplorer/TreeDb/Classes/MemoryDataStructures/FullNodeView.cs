using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using TreeDb.Classes;
using TreeDb.Classes.MemoryDataStructures;
using TreeDb.Classes.Services;

namespace TreeDb
{
    [Serializable]
    public class FullNodeView : TempNodeView
    {
        public string SaveName
        {
            get
            {
                if (string.IsNullOrEmpty(this.saveName))
                {
                    saveName = database.NameFunc(this);
                }
                return saveName;
            }
            set
            {
                this.saveName = value;
            }
        }

        private string saveName;
        private FullNodeView parentNode;

        [JsonIgnore]
        public static Func<string, string, string> PathFunc { get; set; } = (string saveLocation, string saveName) => { return Path.Combine(saveLocation, saveName); };
        [JsonIgnore]
        public static Func<string, FullNodeView> LoadFunc { get; set; } = (string loadPath) =>
        {
            var data = System.IO.File.ReadAllText(loadPath);
            var tableData = JsonSerializer.Deserialize<SerializableTableData>(data);
            return tableData.CreateNodeTable();
        };

        [JsonIgnore]
        public Func<FullNodeView, string> Serialize { get; set; } = (FullNodeView node) => { return JsonSerializer.Serialize(SerializableTableData.CreateFromTable(node)); };

        public FullNodeView(string name, FullNodeView parent, string saveName = null) : base()
        {
            TableReferences = new Dictionary<string, (string name, TypeFileRecord Value)> { };
            parentNode = parent;
            Attributes = new Dictionary<string, object>();
            Attributes.Add(QueryService.PARENT, parentNode);
            Attributes.Add(QueryService.NAME, name);

            if (!string.IsNullOrEmpty(saveName))
            {
                this.SaveName = saveName;
            }
        }

        public void Init(Func<Dictionary<string, (string saveName, TypeFileRecord Props)>> initFunction, string name, FullNodeView parent = null, string[] tags = null)
        {
            TableReferences = initFunction();

            var _tags = tags ?? new string[] { };
            Tags = (Tags ?? new List<string> { });
            Tags.AddRange(_tags);
            Attributes[QueryService.PARENT] = parent;
            Attributes[QueryService.NAME] = name;
        }

        public FullNodeView GetParent()
        {
            return parentNode;
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
            return GetPropertyOrDefault(attributeName);
        }

        public FullNodeView GetOrCreateRecord(string recordName, bool createIfNotExist = false, bool saveIfNotExist = false, string reference = "")
        {
            if (!TableReferences.ContainsKey(recordName))
            {
                FullNodeView table;
                if (!createIfNotExist)
                {
                    return null;
                }

                table = new FullNodeView(recordName, this, reference);
                TableReferences.Add(recordName, (table.SaveName, table));

                if (saveIfNotExist)
                {
                    FullNodeView.database.SaveNode(table, reference);
                }
            }
            return TableReferences[recordName].Props as FullNodeView;
        }

        public void InsertViewRecord(string name, FullNodeView fileRecord, bool saveIfNotExist = false)
        {
            TableReferences[name] = (fileRecord.SaveName, fileRecord);
            fileRecord.InvalidateTable();
            if (saveIfNotExist)
            {
                RunSerializeAndSaveFunc(saveIfNotExist);
            }
        }

        public void InsertViewRecord(string name, bool saveIfNotExist = false)
        {
            FullNodeView fileRecord = new FullNodeView(name, this);
            TableReferences[name] = (fileRecord.SaveName, fileRecord);
            fileRecord.InvalidateTable();
            if (saveIfNotExist)
            {
                InvalidateTable();
            }
        }

        public void PutViewRecordRecursive(string query, bool saveIfNotExist = false, Func<FullNodeView, string> OnCollision = null)
        {
            var (expression, remainingQuery) = QueryService.GetFirstCommand(query, QueryService.QUERY_SEP);
            var name = GetNameFromExpression(expression);

            if (!TableReferences.Keys.Any(Key => Key == name))
            {
                FullNodeView fileRecord = new FullNodeView(name, this);

                TableReferences[name] = (fileRecord.SaveName, fileRecord);
                if (saveIfNotExist)
                {
                    TableReferences[name] = (fileRecord.SaveName, fileRecord);
                    InvalidateTable();
                }
            }
            else
            {
                if (OnCollision != null)
                {
                    OnCollision(TableReferences[name].Props as FullNodeView);
                }
            }

            if (!string.IsNullOrEmpty(remainingQuery))
            {
                (TableReferences[name].Props as FullNodeView).PutViewRecordRecursive(remainingQuery, saveIfNotExist, OnCollision);
            }
        }

        public string GetNameFromExpression(string expression)
        {
            return expression;
        }

        public void DeleteRecord(string key)
        {
            TableReferences.Remove(key);
            InvalidateTable();
        }
        public IEnumerable<FullNodeView> Query(string query, bool createMode = false)
        {
            var path = QueryService.GetFirstCommand(query, Path.DirectorySeparatorChar.ToString());
            var _query = path.command;
            var result = this.GetOrCreateRecord(_query, createMode);
            if (result != null)
            {
                if (string.IsNullOrEmpty(path.remainingQuery))
                {
                    return new List<FullNodeView> { result };
                }
                return result.Query(path.remainingQuery);
            }
            return null;

            //var collection = (IEnumerable<NodeTable>)new List<NodeTable> { this };
            //for (int i = 0; i < path.Length; i++)
            //{
            //    var position = path[i];
            //    collection = collection.Select(x =>
            //    {
            //        return x.GetOrCreateRecord(position, false);
            //    });

            //    return collection;
            //}

            throw new NotImplementedException();
        }

        public object this[string key]
        {
            get
            {
                if (key.StartsWith(QueryService.ATTRIBUTE_QUERY))
                {
                    key = key.Substring(QueryService.ATTRIBUTE_QUERY.Length);
                    return GetAttribute(key);
                }
                if (key.StartsWith(QueryService.TAG_QUERY))
                {
                    key = key.Substring(QueryService.TAG_QUERY.Length);
                    return HasTag(key);
                }
                if (key.StartsWith(QueryService.PARENT_QUERY) || key.Equals(QueryService.PARENT, StringComparison.CurrentCultureIgnoreCase) || key.Equals(".."))
                {
                    return GetParent();
                }
                return GetOrCreateRecord(key, false, false, null);
            }


            set
            {
                if (value is FullNodeView)
                {
                    InsertViewRecord(key, (FullNodeView)value, true);
                }
                else if (value is string)
                {
                    SetAttribute(key, value as string);
                }
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true });
        }

        public void InvalidateTable()
        {
            RunSerializeAndSaveFunc();
        }

        public void RunSerializeAndSaveFunc(bool recursive = false)
        {
            this.SaveName = string.IsNullOrEmpty(SaveName) ? database.NameFunc(this) : SaveName;

            database.SaveNode(this, saveName);

            if (recursive)
            {
                foreach (var table in TableReferences.Values)
                {
                    (table.Props as FullNodeView).RunSerializeAndSaveFunc(recursive);
                }
            }
        }

        public new void Load(int levels = 0)
        {
            var tempTable = database.GetFromReference(SaveName);

            this.TableReferences = tempTable.TableReferences;
            this.Tags = tempTable.Tags;
            this.Attributes = tempTable.Attributes;
        }

        public new void Unload(bool recursive = true)
        {
            if (recursive)
            {
                foreach (var table in TableReferences.Values)
                {
                    table.Props.Unload(recursive);
                }
            }

        }
        public string GetSaveLocation() => PathFunc(database.mainSourceLocation, SaveName);
    }
}