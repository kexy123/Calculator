using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Calculator.Expression.Token
{
    using TokenFunction = Action<Tokenizer, Match>;

    public record TokenDumpFunctions
    {
        public static TokenFunction DumpNumber = (tokenizer, match) =>
        {
            string number = match.Value;
            tokenizer.AddToken(new(TokenType.Number, number));
            tokenizer.Index += number.Length;
        };
    }

    [method: SetsRequiredMembers]
    public struct TokenPattern(string pattern, TokenFunction token)
    {
        public required Regex Pattern = new(@$"\G{Regex.Escape(pattern)}", RegexOptions.Singleline);
        public required TokenFunction Execute = token;
    }

    public record TokenPatterns
    {
        public static TokenPattern[] Patterns = [
            new(@"\d*\.\d*", TokenDumpFunctions.DumpNumber),
            new(@"\d+", TokenDumpFunctions.DumpNumber),
        ];
    }
}
