using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsExplorer_WPF.Misc;

namespace WindowsExplorer_WPF_NET.Misc.Data
{
    public class FileOperationsHelper
    {
        public static long GetSize(string folderPath)
        {
            var totalSize = 0l;
            var folder = new System.IO.DirectoryInfo(folderPath);
            if (!folder.Exists)
            {
                return -1;
            }
            foreach (var file in folder.GetFiles())
            {
                totalSize += file.Length;
            }
            foreach (var dir in folder.GetDirectories())
            {
                totalSize += GetSize(dir.FullName);
            }
            return totalSize;
        }
        public static void CopyItems(List<FFBase> items, string dest)
        {

        }
        public static void MoveItems(List<FFBase> items, string dest)
        {

        }
        public static void RenameItems(List<FFBase> items, string newName, object indexOptions)
        {

        }
        public static void DeleteItems(List<FFBase> items)
        {

        }
        public static long GetDriveCapacity(string drivePath)
        {
            return 0;
        }
        public static long GetDriveUsed(string drivePath)
        {
            return 0;
        }
        public static void SetJpegTags(string path, string[] tags)
        {

        }
        public static IEnumerable<string> GetJpegTags(string path)
        {
            return null;
        }
        public static void SetMpegTags(string path, string[] tags)
        {

        }
        public static IEnumerable<string> GetMpegTags(string path)
        {
            return null;
        }
        public static object GetNTFSFileId(string path) { return null; }
        public static object GetNTFSDirectoryId(string path) { return null; }
        public static object GetRelatedActionsFromRegistry(string path) { return null; }
    }
}
