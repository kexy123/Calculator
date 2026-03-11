using System.Diagnostics.CodeAnalysis;

using Calculator.Value;

namespace Calculator.Operation
{
    [Flags]
    public enum OperatorProperty
    {
        Regular = 0,
        RightToLeft = 1,
        Unary = 2,
        Bracket = 4
    }

    [method: SetsRequiredMembers]
    public struct Operator<A, B, R>(string symbol, OperatorProperty properties, sbyte precedence, Func<A, B, R> function)
    {
        public required string Symbol = symbol;
        public required OperatorProperty Properties = properties;
        public required sbyte Precedence = precedence;
        public required Func<A, B, R> Execute = function;

        public readonly override string ToString() => $"Operator('{Symbol}')";
    }
}
