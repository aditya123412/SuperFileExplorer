using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsExplorer_WPF_NET.Misc.Data;
using System.Text.Json;
using System.IO;
using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF_NET
{
    public class FolderDatabase
    {
        List<(string normalizedPath, FolderTable data)> _folders = new List<(string, FolderTable)>();
        Dictionary<string, string> folderReferences = new Dictionary<string, string>();

        string _storageDirectory, _listsFile;
        public FolderDatabase(string storageDirectory, string folderReferencesFile)
        {
            _storageDirectory = storageDirectory;
            _listsFile = folderReferencesFile;
            folderReferences = JsonSerializer.Deserialize<Dictionary<string, string>>(folderReferencesFile);
        }
        public FolderTable GetFolderTable(string path)
        {
            var normalizedPath = path.Trim().ToLowerInvariant();
            if (_folders.Any(x => x.normalizedPath.Equals(normalizedPath)))
            {
                return _folders.FirstOrDefault(x => x.normalizedPath.Equals(normalizedPath)).data;
            }
            string folderReference = GetFolderReference(normalizedPath);
            if (folderReference == null)
                return null;

            var folderTable = JsonSerializer.Deserialize<FolderTable>(System.IO.File.ReadAllText(Path.Combine(_storageDirectory, folderReference)));
            return folderTable;
        }
        public void SetFolderTable(string path, FolderTable folderTable)
        {
            var normalizedPath = path.Trim().ToLowerInvariant();
            if (folderTable == null || string.IsNullOrEmpty(path))
                return;
            if (!folderReferences.ContainsKey(normalizedPath))
            {
                folderReferences.Add(normalizedPath, Guid.NewGuid().ToString());
            }
            System.IO.File.WriteAllText(Path.Combine(_storageDirectory, folderReferences[normalizedPath]),
                JsonSerializer.Serialize(folderTable));
        }
        public bool ReferenceExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            if (!folderReferences.ContainsKey(path))
                return true;
            return false;
        }
        public void SetFolderTable(string path, Dictionary<string, FFBase> folderData)
        {
            SetFolderTable(path, new FolderTable(folderData));
        }

        private string GetFolderReference(string normalizedPath)
        {
            if (folderReferences.ContainsKey(normalizedPath))
                return folderReferences[normalizedPath];
            else
                return null;
        }
    }
}
