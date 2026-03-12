using Calculator.Expression.Token;
using Calculator.Operation;
using Calculator.Value;
using Calculator.Variable;
using System.Diagnostics.CodeAnalysis;

namespace Calculator.Expression
{
    public interface ICalculatorContext
    {
        public Operator[] Operators { get; }
        public TokenPattern[] TokenPatterns { get; }
    }

    [method: SetsRequiredMembers]
    public class CalculatorContext(ICalculatorContext context) : IVariableContext, IOperatorContext
    {
        public Dictionary<string, Function> Functions => [];
        public Dictionary<string, IValue> Variables => [];

        public void AssignFunction(Function function) => Functions.Add(function.Name, function);
        public void AssignVariable(string name, IValue value) => Variables.Add(name, value);

        public Operator[] Operators => context.Operators;

        public Operator DetermineOperationFromString(string source)
        {
            foreach (Operator operatorObject in Operators)
            {
                if (source.StartsWith(operatorObject.Symbol)) return operatorObject;
            }
            throw new ArgumentException($"No operator matches '{source}'.");
        }

        public required TokenPattern[] Patterns = context.TokenPatterns;

        private Tokenizer? tokenizer;
        public Tokenizer Tokenizer => tokenizer ??= new(this);

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
    }
}
