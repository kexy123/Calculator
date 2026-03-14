namespace Core.Expression.Parser
{
    public interface IParser
    {
        public List<Token.Token> Output { get; }

        /// <summary>
        /// Parses the given array of tokens to easily readable formats.
        /// </summary>
        /// <param name="tokens">The array of tokens to parse.</param>
        public void Parse(in Token.Token[] tokens, CalculatorContext context);
    }
}
