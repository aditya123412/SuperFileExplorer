using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using FILETIME = System.Runtime.InteropServices.FILETIME;

namespace Classes.Common
{
    public static class HelperFunctions
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        public struct BY_HANDLE_FILE_INFORMATION
        {
            public uint FileAttributes;
            public FILETIME CreationTime;
            public FILETIME LastAccessTime;
            public FILETIME LastWriteTime;
            public uint VolumeSerialNumber;
            public uint FileSizeHigh;
            public uint FileSizeLow;
            public uint NumberOfLinks;
            public uint FileIndexHigh;
            public uint FileIndexLow;
        }

        public static ulong GetFileId(this FileInfo file)
        {
            try
            {
                HelperFunctions.BY_HANDLE_FILE_INFORMATION objectFileInfo = new HelperFunctions.BY_HANDLE_FILE_INFORMATION();

                FileInfo fi = new FileInfo(file.FullName);
                FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                HelperFunctions.GetFileInformationByHandle(fs.Handle, out objectFileInfo);

                fs.Close();

                ulong fileIndex = ((ulong)objectFileInfo.FileIndexHigh << 32) + (ulong)objectFileInfo.FileIndexLow;

                return fileIndex;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static (string command, string remainingQuery) GetFirstCommand(string Query, string QuerySeparator)
        {
            string remainingQuery = default;
            var commands = Query.Split(QuerySeparator[0]);
            if (commands.Length == 1)
            {
                return (commands[0], default);
            }
            remainingQuery = String.Join(QuerySeparator, commands.Skip(1));
            return (commands[0], remainingQuery);
        }
    }
}
