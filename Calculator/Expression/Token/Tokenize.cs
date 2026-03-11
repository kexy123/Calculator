using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Calculator.Expression.Token
{
    [method: SetsRequiredMembers]
    public class Tokenizer(string source, CalculatorContext context, TokenPattern[] tokenPatterns)
    {
        public required string Source = source;
        public required CalculatorContext Context = context;
        public required TokenPattern[] TokenPatterns = tokenPatterns;

        readonly List<Token> TokenList = [];
        public Token[] Tokens => [.. TokenList];

        public int Index = 0;

        /// <summary>
        /// Adds a token to the Tokens.
        /// </summary>
        /// <param name="token">The token to add.</param>
        public void AddToken(Token token) => TokenList.Add(token);

        /// <summary>
        /// Gets the next token in the source and adds it to the Tokens.
        /// </summary>
        /// <exception cref="InvalidDataException">There exists no pattern that can parse that section.</exception>
        public void NextToken()
        {
            foreach (TokenPattern pattern in TokenPatterns)
            {
                Match match = pattern.Pattern.Match(Source, Index);
                Console.WriteLine(pattern.Pattern);
                if (!match.Success) continue;
                pattern.Execute(this, match);
                return;
            }
            throw new InvalidDataException($"Index {Index} of expression '{Source}' cannot be parsed.");
        }

        /// <summary>
        /// Tokenizes the equation.
        /// </summary>
        public void Tokenize()
        {
            while (Index < Source.Length) NextToken();
        }

        // TODO: Refactor this into a Console solution.
        public override string ToString()
        {
            string result = "";
            uint index = 0;
            foreach (Token token in TokenList)
            {
                result += index > 0 ? ", " + token : token;
                index++;
            }
            return result;
        }
    }
}
