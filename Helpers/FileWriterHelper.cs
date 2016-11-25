using System;
using System.IO;

namespace Securitytesting.Helpers
{
    public static class FileWriterHelper
    {
        public static void WriteReport(string path, string data)
        {
            if (path.EndsWith(@"\") == false) path += @"\";
            var fileName = "Report-" + DateTime.Now + ".html";
            fileName = fileName.Replace(':', '_');
            File.WriteAllText(path + fileName, data);
        }
    }
}
