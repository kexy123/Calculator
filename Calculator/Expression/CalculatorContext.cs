using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Operation;
using Core.Value;
using Core.Variable;
using System.Diagnostics.CodeAnalysis;

namespace Core.Expression
{
    public interface ICalculatorContext
    {
        public Operator[] Operators { get; }
        public TokenPattern[] TokenPatterns { get; }
        public IParser Parser { get; }
    }

    [method: SetsRequiredMembers]
    public class CalculatorContext(ICalculatorContext context) : IVariableContext, IOperatorContext
    {
        private readonly Dictionary<string, Function> functions = [];
        private readonly Dictionary<string, IValue> variables = [];

        public Dictionary<string, Function> Functions => functions;
        public Dictionary<string, IValue> Variables => variables;

        public void AssignFunction(Function function) => Functions.Add(function.Name, function);
        public void AssignVariable(string name, IValue value)
        {
            value.SetAsVariable(name);
            Variables.Add(name, value);
        }

        public IValue GetVariableFromName(string name)
        {
            string sub = "";
            int index = 0;
            IValue? value;
            do
            {
                if (index >= name.Length) return new Nothing(name);
                sub += name[index];
                index++;
            } while (!Variables.TryGetValue(sub, out value));
            return value;
        }

        public Operator[] Operators => context.Operators;

        /// <summary>
        /// Determines the first operator that matches the beginning of the specified string.
        /// </summary>
        /// <param name="source">The input string to evaluate for a matching operator.</param>
        /// <returns>The first operator whose symbol matches the start of the input string.</returns>
        /// <exception cref="ArgumentException">Thrown if no operator matches the beginning of the specified string.</exception>
        public Operator DetermineOperationFromString(string source)
        {
            foreach (Operator operatorObject in Operators)
            {
                if (source.StartsWith(operatorObject.Symbol)) return operatorObject;
            }
            throw new ArgumentException($"No operator matches '{source}'");
        }

        public required TokenPattern[] Patterns = context.TokenPatterns;

        private Tokenizer? tokenizer;
        private IParser? parser;
        public Tokenizer Tokenizer => tokenizer ??= new(this);
        public IParser Parser => parser ??= context.Parser;

        /// <summary>
        /// Returns the token list from a given expression.
        /// </summary>
        /// <param name="expression">The expression to tokenize.</param>
        /// <returns>The token list as an array.</returns>
        public Token.Token[] TokenizeExpression(string expression)
        {
            Tokenizer.Tokenize(expression);
            return Tokenizer.Tokens;
        }

        /// <summary>
        /// Parses the specified sequence of tokens as an expression and returns the resulting token array.
        /// </summary>
        /// <param name="tokens">An array of tokens representing the input expression to parse.</param>
        /// <returns>An array of tokens representing the parsed expression.</returns>
        public Token.Token[] Parse(Token.Token[] tokens)
        {
            Parser.Parse(tokens, this);
            return [.. Parser.Output];
        }

        /// <summary>
        /// Evalutes the given list of tokens.
        /// </summary>
        /// <param name="tokens">The tokens to evaluate.</param>
        /// <returns>The value returned.</returns>
        public IValue EvaluateTokens(Token.Token[] tokens) => Evaluation.Evaluator.Evaluate(tokens, this);

        /// <summary>
        /// Evaluates the given expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <returns>The value returned.</returns>
        public IValue Evaluate(string expression) => EvaluateTokens(Parse(TokenizeExpression(expression)));
    }
}
