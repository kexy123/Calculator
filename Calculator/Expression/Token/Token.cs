using System.Diagnostics.CodeAnalysis;

namespace Calculator.Expression.Token
{
    public enum TokenType
    {
        Number,
        Operator,
        Variable,
        Whitespace
    }

    [method: SetsRequiredMembers]
    public struct Token(TokenType type, string source)
    {
        public required TokenType Type = type;
        public required string Source = source;

        public override readonly string ToString() => $"'{Source}'; {Type}";
    }
}
