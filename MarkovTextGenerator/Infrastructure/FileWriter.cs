using System;
using System.IO;

namespace MarkovTextGenerator.Infrastructure
{
    public class FileWriter
    {
        public bool Write(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
