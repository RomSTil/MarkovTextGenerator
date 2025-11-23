using System;
using System.Collections.Generic;

namespace MarkovTextGenerator.Core
{
    public class Tokenizer
    {
        public List<string> Tokenize(string text)
        {
            var words = new List<string>();

            if (string.IsNullOrWhiteSpace(text))
                return words;
            string[] split = text.Split(' ');

            foreach (var word in split)
            {
                string trimmed = word.Trim();

                if (!string.IsNullOrWhiteSpace(trimmed))
                    words.Add(trimmed);
            }

            return words;
        }
    }
}
