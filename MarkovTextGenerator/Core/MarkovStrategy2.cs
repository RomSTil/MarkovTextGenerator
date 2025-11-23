using System.Collections.Generic;

namespace MarkovTextGenerator.Core
{
    public class MarkovStrategy2 : IMarkovStrategy
    {
        public MarkovModel BuildModel(List<string> words)
        {
            var transitions = new Dictionary<string, List<string>>();

            if (words == null || words.Count < 3)
                return new MarkovModel(transitions);

            for (int i = 0; i < words.Count - 2; i++)
            {
                string key = words[i] + " " + words[i + 1];
                string next = words[i + 2];

                if (!transitions.ContainsKey(key))
                    transitions[key] = new List<string>();

                transitions[key].Add(next);
            }

            return new MarkovModel(transitions);
        }
    }
}
