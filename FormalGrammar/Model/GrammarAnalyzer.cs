using System.Linq;
using FormalGrammar.Utils;

namespace FormalGrammar.Model
{
    public class GrammarAnalyzer
    {
        public readonly bool IsSimplePrecedence = false;
        public readonly bool IsWeakPrecedence = false;
        public GrammarAnalyzer(GrammarPrecedence grammarPrecedence)
        {
            this.grammarPrecedence = grammarPrecedence;
            IsSimplePrecedence = grammarPrecedence.IsSimplePrecedence();
            IsWeakPrecedence = IsSimplePrecedence || grammarPrecedence.IsWeakPrecedence();
        }

        private readonly GrammarPrecedence grammarPrecedence;
    }
}