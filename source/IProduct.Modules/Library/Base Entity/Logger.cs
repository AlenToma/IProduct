using EntityWorker.Core.Helper;
using EntityWorker.Core.Interface;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IProduct.Modules.Library.Base_Entity
{
    public class Logger : ILog
    {
        private string logIdentifier = $"{DateTime.Now.ToString("yyyy-MM-dd")} Ilog.txt";
        private string logPath = AppDomain.CurrentDomain.BaseDirectory;
        private StringBuilder text = new StringBuilder();
        public Logger()
        {
            DirectoryInfo dinfo = new DirectoryInfo(logPath);
            var files = dinfo.GetFiles("*.txt");
            foreach (FileInfo file in files)
            {

                var name = file.Name.Split(' ')[0];
                if (name.ConvertValue<DateTime?>().HasValue && file.Name.Contains("Ilog"))
                {

                    if (name.ConvertValue<DateTime>().Date == DateTime.Now.Date)
                    {
                        logIdentifier = file.Name;
                        break;
                    }
                }
            }

            logIdentifier = Path.Combine(logPath, logIdentifier);
            using (var stream = File.Open(logIdentifier, FileMode.OpenOrCreate)) { }

        }

        public void Dispose()
        {
        }

        public void Error(Exception exception)
        {
            lock (this)
            {
                using (StreamWriter stream = new StreamWriter(logIdentifier, append: true))
                    stream.WriteLine($"Error:{DateTime.Now} - {exception.Message}");
            }

        }

        public void Info(string message, object infoData)
        {
#if DEBUG
            lock (this)
            {
                using (StreamWriter stream = new StreamWriter(logIdentifier, append: true))
                    stream.WriteLine($"Info:{DateTime.Now} - {message} - \n {infoData}");
            }
#endif
        }
    }
}
