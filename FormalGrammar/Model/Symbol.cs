namespace FormalGrammar.Model
{
    public abstract class Symbol
    {
        public readonly char Value;
        public abstract bool IsTerminal { get; }

        protected Symbol(char value) => Value = value;

        public override bool Equals(object obj) => obj != null && obj is Symbol s && s.IsTerminal == IsTerminal && s.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"{Value}";

        public static bool operator ==(Symbol a, Symbol b) => ReferenceEquals(a, b) || (a?.Equals(b) ?? false);
        public static bool operator !=(Symbol a, Symbol b) => !(a == b);
    }

    public class Terminal : Symbol
    {
        public Terminal(char value) : base(value) { }
        public override bool IsTerminal { get; } = true;
    }

    public class NonTerminal : Symbol
    {
        public NonTerminal(char value) : base(value) { }
        public override bool IsTerminal { get; } = false;
    }
}