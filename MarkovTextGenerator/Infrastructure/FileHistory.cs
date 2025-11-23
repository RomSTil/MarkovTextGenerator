using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MarkovTextGenerator.Infrastructure
{
    public class FileHistory
    {
        private readonly string _historyPath;
        private readonly int _maxEntries;

        public class HistoryEntry
        {
            public string DisplayName { get; set; } 
            public string FilePath { get; set; } 
        }

        public FileHistory(string historyFilename = "file_history.json", int maxEntries = 10)
        {
            _historyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, historyFilename);
            _maxEntries = maxEntries;

            if (!File.Exists(_historyPath))
            {
                File.WriteAllText(_historyPath, "[]");
            }
        }

        public List<HistoryEntry> LoadHistory()
        {
            try
            {
                string json = File.ReadAllText(_historyPath);
                var list = JsonConvert.DeserializeObject<List<HistoryEntry>>(json)
                           ?? new List<HistoryEntry>();

                list.RemoveAll(e => !File.Exists(e.FilePath));

                return list;
            }
            catch
            {
                return new List<HistoryEntry>();
            }
        }

        public void AddPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;

            var history = LoadHistory();

            string display = Path.GetFileNameWithoutExtension(path);

            history.RemoveAll(h => h.FilePath == path);

            history.Insert(0, new HistoryEntry
            {
                DisplayName = display,
                FilePath = path
            });

            if (history.Count > _maxEntries)
                history.RemoveRange(_maxEntries, history.Count - _maxEntries);

            string json = JsonConvert.SerializeObject(history, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_historyPath, json);
        }
    }
}
