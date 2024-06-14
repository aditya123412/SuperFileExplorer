using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDb.Classes.Services
{
    public class Logger
    {
        public string LogSavePath { get; set; }
        private List<string> _logs = new List<string>();

        public Logger(string path)
        {
            LogSavePath = path;
        }

        void Log(string log)
        {
            _logs.Add(log);
        }

        void Save()
        {
            System.IO.File.AppendAllLines(LogSavePath, _logs);
        }
    }
}
