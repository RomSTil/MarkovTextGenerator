using MarkovTextGenerator.Core;
using MarkovTextGenerator.Infrastructure;

namespace MarkovTextGenerator.Builder
{
    public class TextGeneratorBuilder
    {
        private string _filePath;
        private int _length = 100;
        private int _order = 1;

        private ITextPreprocessor _preprocessor;
        private Tokenizer _tokenizer;
        private IMarkovStrategy _strategy;
        private FileReader _reader;

        public TextGeneratorBuilder()
        {
            // Значения по умолчанию
            _preprocessor = new TextPreprocessor();
            _tokenizer = new Tokenizer();
            _reader = new FileReader();
            _strategy = new MarkovStrategy1();
        }

        // --------------------------------------------------------
        //       МЕТОДЫ КОНФИГУРАЦИИ BUILDER-А
        // --------------------------------------------------------

        public TextGeneratorBuilder SetFile(string filePath)
        {
            _filePath = filePath;
            return this;
        }

        public TextGeneratorBuilder SetLength(int length)
        {
            _length = length;
            return this;
        }

        public TextGeneratorBuilder SetOrder(int order)
        {
            _order = order;

            if (order == 1)
                _strategy = new MarkovStrategy1();
            else
                _strategy = new MarkovStrategy2();

            return this;
        }

        public TextGeneratorBuilder SetPreprocessor(ITextPreprocessor preprocessor)
        {
            _preprocessor = preprocessor;
            return this;
        }

        public TextGeneratorBuilder SetTokenizer(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            return this;
        }

        public TextGeneratorBuilder SetStrategy(IMarkovStrategy strategy)
        {
            _strategy = strategy;
            return this;
        }

        public TextGeneratorBuilder SetReader(FileReader reader)
        {
            _reader = reader;
            return this;
        }

        // --------------------------------------------------------
        //                    Build()
        // --------------------------------------------------------
        public TextGenerator Build()
        {
            return new TextGenerator(
                filePath: _filePath,
                length: _length,
                preprocessor: _preprocessor,
                tokenizer: _tokenizer,
                strategy: _strategy,
                reader: _reader
            );
        }
    }
}
