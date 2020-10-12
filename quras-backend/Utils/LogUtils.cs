using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    internal sealed partial class LogUtils
    {
        public static LogUtils Instance = null;

        private string LogDictionary = "";

        private object LOG_LOCK = new object();

        public static LogUtils Default
        {
            get
            {
                if (Instance == null)
                    Instance = new LogUtils();

                return Instance;
            }
        }

        public LogUtils()
        {
            LogDictionary = Path.Combine(AppContext.BaseDirectory, "Logs");
        }

        public void Log(String message)
        {
            DateTime now = DateTime.Now;
            string line = $"[{now:yyyy-MM-dd HH\\:mm\\:ss}] {message}";
            Console.WriteLine(line);
            if (string.IsNullOrEmpty(LogDictionary)) return;
            lock (LogDictionary)
            {
                Directory.CreateDirectory(LogDictionary);
                string path = Path.Combine(LogDictionary, $"{now:yyyy-MM-dd}.log");
                lock (LOG_LOCK)
                {
                    File.AppendAllLines(path, new[] { line });
                }
            }
        }
    }
}
