using Core.Value;
using System.Diagnostics.CodeAnalysis;

namespace Core.Variable
{
    using Form = Func<IValue[], IValue>;
    using ParameterMap = string[];

    public enum FunctionType
    {
        CSharpFunction,
        Expression
    }

    // TODO: consider overloading functions with different parameter counts.
    public struct Function
    {
        public required string Name;
        public FunctionType Type;

        public Form? Execute;
        public string? Expression;

        public ParameterMap? ParameterMap;
        public int ParameterCount;

        /// <summary>
        /// Creates a function with the given name, C# function and parameter count.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="function">The C# function to embed.</param>
        /// <param name="parameters">The number of parameters the function has in order.</param>
        [SetsRequiredMembers]
        public Function(string name, Form function, int parameters)
        {
            Name = name;
            Execute = function;
            ParameterCount = parameters;
            Type = FunctionType.CSharpFunction;
        }

        /// <summary>
        /// Creates a function with the given name, expression and parameter map.
        /// The expression should be in terms of the parameters defined in the parameter map.
        /// For example, if the parameter map is [ "x", "y" ], then the expression could be "x + y" where the function is "f(x, y)".
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="expression">The mathematical expression in terms of the parameter map.</param>
        /// <param name="parameterMap">The parameter map.</param>
        [SetsRequiredMembers]
        public Function(string name, string expression, ParameterMap parameterMap)
        {
            Name = name;
            Expression = expression;
            ParameterMap = parameterMap;
            ParameterCount = parameterMap.Length;
            Type = FunctionType.Expression;
        }

        public readonly override string ToString() => $"Function('{Name}')"; // TODO: improve this to include parameters and expression if applicable.
    }
}
