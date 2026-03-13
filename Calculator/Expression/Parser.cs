namespace Core.Expression
{
    public interface IParser
    {
        public List<Token.Token> Output { get; }
        public Token.Token[] Tokens { get; }

        /// <summary>
        /// Parses the given array of tokens to easily readable formats.
        /// </summary>
        /// <param name="tokens">The array of tokens to parse.</param>
        public void Parse(Token.Token[] tokens);
    }
}
