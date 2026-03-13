using Core.Expression;
using Core.Expression.Token;

namespace Core.AssetParsers
{
    public class Arithmetic : IParser
    {
        public readonly List<Token> Output = [];
        List<Token> IParser.Output => Output;

        public Token[] Tokens = [];
        Token[] IParser.Tokens => Tokens;

        public void Parse(Token[] tokens)
        {
            Output.Clear();
            Tokens = tokens;
            // TODO: Create Arithmetic parser here.
        }
    }
}
