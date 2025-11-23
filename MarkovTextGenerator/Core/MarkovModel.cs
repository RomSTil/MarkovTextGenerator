using System;
using System.Collections.Generic;
using System.Text;

namespace MarkovTextGenerator.Core
{
    public class MarkovModel
    {
        private readonly Dictionary<string, List<string>> _transitions;
        private readonly Random _random;

        public MarkovModel(Dictionary<string, List<string>> transitions)
        {
            _transitions = transitions ?? new Dictionary<string, List<string>>();
            _random = new Random();
        }

        // --------------------------------------------------------
        // Основной метод генерации текста
        // --------------------------------------------------------
        public string GenerateText(int length)
        {
            if (_transitions.Count == 0)
                return string.Empty;

            string currentKey = GetRandomKey();

            var result = new List<string>();
            result.AddRange(currentKey.Split(' '));

            for (int i = 0; i < length; i++)
            {
                if (!_transitions.ContainsKey(currentKey) ||
                    _transitions[currentKey].Count == 0)
                {
                    currentKey = GetRandomKey();
                }

                var nextWord = GetRandomNextWord(currentKey);

                result.Add(nextWord);

                currentKey = ShiftKey(currentKey, nextWord);
            }

            return string.Join(" ", result);
        }

        // --------------------------------------------------------
        // Берём случайный ключ из словаря переходов
        // --------------------------------------------------------
        private string GetRandomKey()
        {
            int index = _random.Next(_transitions.Count);
            int i = 0;

            foreach (var key in _transitions.Keys)
            {
                if (i == index)
                    return key;
                i++;
            }

            return "";
        }

        // --------------------------------------------------------
        // Выбираем случайное следующее слово из списка
        // --------------------------------------------------------
        private string GetRandomNextWord(string key)
        {
            var list = _transitions[key];
            int index = _random.Next(list.Count);
            return list[index];
        }

        // --------------------------------------------------------
        // Сдвигаем ключ для стратегии 2-го порядка:
        // "a b" + "c" => "b c"
        // Для стратегии 1-го порядка просто "c".
        // --------------------------------------------------------
        private string ShiftKey(string currentKey, string nextWord)
        {
            string[] parts = currentKey.Split(' ');

            if (parts.Length == 1)
                return nextWord;

            var newParts = new string[parts.Length];
            for (int i = 1; i < parts.Length; i++)
            {
                newParts[i - 1] = parts[i];
            }
            newParts[parts.Length - 1] = nextWord;

            return string.Join(" ", newParts);
        }
    }
}
