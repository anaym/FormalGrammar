namespace FormalGrammar.Model
{
    public abstract class Symbol
    {
        public abstract char Value { get; }

        public override bool Equals(object obj) => obj != null && obj is Symbol s && s.GetType() == GetType() && s.Value == Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"{Value}";

        public static bool operator ==(Symbol a, Symbol b) => ReferenceEquals(a, b) || (a?.Equals(b) ?? false);
        public static bool operator !=(Symbol a, Symbol b) => !(a == b);
    }

    public class Terminal : Symbol
    {
        public Terminal(char value) => Value = value;
        public override char Value { get; }
    }

    public class NonTerminal : Symbol
    {
        public NonTerminal(char value) => Value = value;
        public override char Value { get; }
    }

    public class Start : Symbol
    {
        public override char Value { get; } = '^';
    }

    public class End : Symbol
    {
        public override char Value { get; } = '$';
    }
}