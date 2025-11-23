using System;
using System.Text.RegularExpressions;

namespace MarkovTextGenerator.Core
{
    public interface ITextPreprocessor
    {
        string Preprocess(string text);
    }
public class TextPreprocessor : ITextPreprocessor
    {
        public string Preprocess(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            text = text.ToLower();
            text = Regex.Replace(text, @"[^a-zа-я0-9ё\s.,!?-]", " ");
            text = Regex.Replace(text, @"\s+", " ");
            text = Regex.Replace(text, @"[^a-zа-яё\s]", " ");
            text = text.Replace("ё", "е");
            text = text.Trim();

            return text;
        }
    }
}
