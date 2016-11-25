using System;
using System.IO;

namespace Securitytesting.Helpers
{
    public static class FileWriterHelper
    {
        public static void WriteFile(string path, string data)
        {
            var fileName = "Report-" + DateTime.Now + ".html";
            fileName = fileName.Replace(':', '_');
            File.WriteAllText(path + fileName, data);
        }
    }
}
