using System.Collections.Generic;

namespace MarkovTextGenerator.Core
{
    public class MarkovStrategy1 : IMarkovStrategy
    {
        public MarkovModel BuildModel(List<string> words)
        {
            var transitions = new Dictionary<string, List<string>>();

            if (words == null || words.Count < 2)
                return new MarkovModel(transitions);

            for (int i = 0; i < words.Count - 1; i++)
            {
                string current = words[i];
                string next = words[i + 1];

                if (!transitions.ContainsKey(current))
                    transitions[current] = new List<string>();

                transitions[current].Add(next);
            }

            return new MarkovModel(transitions);
        }
    }
}
