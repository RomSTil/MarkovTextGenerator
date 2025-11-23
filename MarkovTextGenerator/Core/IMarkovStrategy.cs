using System.Collections.Generic;

namespace MarkovTextGenerator.Core
{
    public interface IMarkovStrategy
    {
        MarkovModel BuildModel(List<string> words);
    }
}
