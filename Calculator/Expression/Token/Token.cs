using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Core.Value;

namespace Core.Expression.Token
{
    using TokenFunction = Action<Tokenizer, Match>;

    [method: SetsRequiredMembers]
    public struct TokenPattern([StringSyntax("Regex")] string pattern, TokenFunction token)
    {
        public required Regex Pattern = new(@$"\G{pattern}", RegexOptions.Singleline);
        public required TokenFunction Execute = token;
    }

    [Flags]
    public enum TokenType
    {
        Operand = 1,
        Number = 3,
        Variable = 5,

        Operator = 8,
        Function = 17,

        Whitespace = 32,
    }

    [method: SetsRequiredMembers]
    public struct Token(TokenType type, string source, IValue? value = null)
    {
        public required TokenType Type = type;
        public required string Source = source;
        public IValue? Value = value;

        /// <summary>
        /// Determines whether the specified property or set of properties is present.
        /// </summary>
        /// <param name="property">The property or combination of properties to check for within the Token.</param>
        /// <returns>true if all specified properties are present, and false if not.</returns>
        public readonly bool ContainsProperty(TokenType property) => Type.HasFlag(property);

        public override readonly string ToString() => $"'{Source}'; {Type}";
    }
}
