using System;
using System.Collections.Generic;

namespace MarkovTextGenerator.Core
{
    public class TextGenerator
    {
        private readonly string _filePath;
        private readonly int _length;
        private readonly ITextPreprocessor _preprocessor;
        private readonly Tokenizer _tokenizer;
        private readonly IMarkovStrategy _strategy;
        private readonly Infrastructure.FileReader _reader;

        public TextGenerator(
            string filePath,
            int length,
            ITextPreprocessor preprocessor,
            Tokenizer tokenizer,
            IMarkovStrategy strategy,
            Infrastructure.FileReader reader)
        {
            _filePath = filePath;
            _length = length;
            _preprocessor = preprocessor;
            _tokenizer = tokenizer;
            _strategy = strategy;
            _reader = reader;
        }

        // -------------------------------------------------
        // генерация текста
        // -------------------------------------------------
        public string Generate()
        {
            string rawText = _reader.Read(_filePath);
            if (string.IsNullOrWhiteSpace(rawText))
                return "Ошибка: файл пуст или недоступен.";
            string cleaned = _preprocessor.Preprocess(rawText);
            List<string> words = _tokenizer.Tokenize(cleaned);
            if (words.Count < 3)
                return "Ошибка: слишком мало текста для генерации.";
            MarkovModel model = _strategy.BuildModel(words);
            return model.GenerateText(_length);
        }
    }
}
