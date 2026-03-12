using System.Diagnostics.CodeAnalysis;

using Core.Value;

namespace Core.Operation
{
    using Form = Func<IValue, IValue, IValue>;

    [Flags]
    public enum OperatorProperty
    {
        Regular = 0,
        RightToLeft = 1,
        Unary = 2,
        Bracket = 4,
        Transitive = 8,
    }

    [method: SetsRequiredMembers]
    public struct Operator(string symbol, OperatorProperty properties, sbyte precedence, Form function)
    {
        public required string Symbol = symbol;
        public required OperatorProperty Properties = properties;
        public required sbyte Precedence = precedence;
        public required Form Execute = function;

        public readonly override string ToString() => $"Operator('{Symbol}')";
    }
}
