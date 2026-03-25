using Core.Expression;
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

    public struct Function
    {
        public required string Name;
        public FunctionType Type;

        public Form? Execute;
        public string? Expression;

        public ParameterMap? ParameterMap;
        public int ParameterCount;

        /// <summary>
        /// Invokes the function with the given arguments. If the function is a C# function, it will execute the C# function. If the function is an expression, it will evaluate the expression in a new context with the parameters assigned to the arguments.
        /// </summary>
        /// <param name="context">The CalculatorContext.</param>
        /// <param name="arguments">The argument list.</param>
        /// <returns>The function's result.</returns>
        public readonly IValue Invoke(CalculatorContext context, IValue[] arguments)
        {
            if (Type == FunctionType.CSharpFunction) return Execute!(arguments);

            Dictionary<string, IValue> parameterMap = [];
            foreach (IValue value in arguments) parameterMap.Add(ParameterMap![parameterMap.Count], value);
            return context.EvaluateUnderScope(Expression!, parameterMap);
        }

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

        /// <summary>
        /// Creates a function based on another function, but with a different parameter count. This is used for default functions, where the default function has a parameter map that accepts any number of parameters, and the new function has a parameter map that accepts a specific number of parameters. The expression of the new function is the same as the default function, but with the parameters in the parameter map replaced by the corresponding arguments in the new parameter map.
        /// </summary>
        /// <param name="function">The function to be based on.</param>
        /// <param name="parameters">The new parameter count.</param>
        [SetsRequiredMembers]
        public Function(Function function, int parameters)
        {
            Name = function.Name;
            Type = function.Type;
            Execute = function.Execute;
            Expression = function.Expression;
            ParameterMap = function.ParameterMap;
            ParameterCount = parameters;
        }

        public readonly override string ToString() => $"{Name}(... {ParameterCount})"; // TODO: improve this to include expression if applicable.
    }
}
