using System;
using System.IO;

namespace MarkovTextGenerator.Infrastructure
{
    public class FileReader
    {
        public string Read(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            if (!File.Exists(path))
                return string.Empty;

            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
