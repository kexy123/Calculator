using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Operation;
using Core.Value;
using Core.Variable;
using System.Diagnostics.CodeAnalysis;

namespace Core.Expression
{
    using FunctionOverloadList = Dictionary<int, Function>;

    public interface ICalculatorContext
    {
        public Operator[] Operators { get; }
        public TokenPattern[] TokenPatterns { get; }
        public IParser Parser { get; }
    }

    [method: SetsRequiredMembers]
    public class CalculatorContext(ICalculatorContext context) : IVariableContext, IOperatorContext
    {
        private readonly Dictionary<string, FunctionOverloadList> functions = [];
        private readonly Dictionary<string, IValue> variables = [];

        public Dictionary<string, FunctionOverloadList> Functions => functions;
        public Dictionary<string, IValue> Variables => variables;

        public void AssignFunction(Function function)
        {
            if (Functions.TryGetValue(function.Name, out FunctionOverloadList? functionDict))
            {
                if (!functionDict.TryAdd(function.ParameterCount, function)) functionDict[function.ParameterCount] = function;
            }
            else
            {
                Functions.Add(function.Name, new() { { function.ParameterCount, function } });
            }
        }
        public void AssignVariable(string name, IValue value)
        {
            value.SetAsVariable(name);
            if (!Variables.TryAdd(name, value)) Variables[name] = value;
        }

        public FunctionOverloadList? GetFunctionFromName(string name, out string result)
        {
            result = "";
            string sub = "";
            int index = 0;
            FunctionOverloadList? value;
            do
            {
                if (index >= name.Length) return null;
                sub += name[index];
                index++;
            } while (!Functions.TryGetValue(sub, out value));
            result = sub;
            return value;
        }
        public Function GetFunction(FunctionOverloadList overloads, int args)
        {
            if (overloads.TryGetValue(args, out Function function)) return function;
            else if (overloads.TryGetValue(-1, out Function defaultFunction)) return new Function(defaultFunction, args);
            else throw new MathArgumentException($"Function doesn't accept {args} parameter(s)");
        }
        public IValue GetVariableFromName(string name)
        {
            string sub = "";
            int index = 0;
            IValue? value;
            do
            {
                if (index >= name.Length) return new NothingToken(name);
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

        public Tokenizer Tokenizer => new(this);
        public IParser Parser => context.Parser;

        /// <summary>
        /// Returns the token list from a given expression.
        /// </summary>
        /// <param name="expression">The expression to tokenize.</param>
        /// <returns>The Tokenizer.</returns>
        public Tokenizer TokenizeExpression(string expression)
        {
            Tokenizer tokenizer = Tokenizer;
            tokenizer.Tokenize(expression);
            return tokenizer;
        }

        /// <summary>
        /// Parses the specified sequence of tokens as an expression and returns the resulting token array.
        /// </summary>
        /// <param name="tokenizer">The tokenizer to parse.</param>
        /// <returns>The Parser.</returns>
        public IParser Parse(Tokenizer tokenizer)
        {
            IParser parser = Parser;
            parser.Parse(tokenizer.Tokens, this);
            return parser;
        }

        /// <summary>
        /// Evalutes the given list of tokens.
        /// </summary>
        /// <param name="parser">The parser to go over.</param>
        /// <returns>The value returned.</returns>
        public IValue EvaluateTokens(IParser parser) => Evaluation.Evaluator.Evaluate([.. parser.Output], this);

        /// <summary>
        /// Evaluates the given expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <returns>The value returned.</returns>
        public IValue Evaluate(string expression) => EvaluateTokens(Parse(TokenizeExpression(expression)));
    }
}
