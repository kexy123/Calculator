using Core.Value;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Core.Expression.Token
{
    using TokenFunction = Action<Tokenizer, Match>;

    [method: SetsRequiredMembers]
    public struct TokenPattern(string pattern, TokenFunction token)
    {
        public required Regex Pattern = new(@$"\G{pattern}", RegexOptions.Singleline);
        public required TokenFunction Execute = token;
    }

    public enum TokenType
    {
        Number,
        Operator,
        Variable,
        Whitespace
    }

    [method: SetsRequiredMembers]
    public struct Token(TokenType type, string source, IValue? value = null)
    {
        public required TokenType Type = type;
        public required string Source = source;
        public IValue? Value = value;

        public override readonly string ToString() => $"'{Source}'; {Type}";
    }
}
