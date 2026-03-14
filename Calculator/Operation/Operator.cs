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

        UnaryLeft = 2,
        UnaryRight = 6,

        Transitive = 8,

        OpenBracket = 16,
        ClosedBracket = 32,

        Separator = 64,
    }

    [method: SetsRequiredMembers]
    public struct Operator(string symbol, OperatorProperty properties, sbyte precedence, Form function)
    {
        public required string Symbol = symbol;
        public required OperatorProperty Properties = properties;
        public required sbyte Precedence = precedence;
        public required Form Execute = function;

        public override readonly bool Equals(object? obj) => obj is Operator operatorObject && Symbol == operatorObject.Symbol;
        public override readonly int GetHashCode() => Symbol.GetHashCode();

        /// <summary>
        /// Determines whether two Operators are equal by their symbols.
        /// </summary>
        /// <param name="left">The operator on the left to check.</param>
        /// <param name="right">The operator on the right to check.</param>
        /// <returns>True if they have the same symbol, and false if not.</returns>
        public static bool operator ==(Operator left, Operator right) => left.Symbol == right.Symbol;
        public static bool operator !=(Operator left, Operator right) => left.Symbol != right.Symbol;

        public static bool operator >(Operator left, Operator right) => left.Precedence > right.Precedence;
        public static bool operator >=(Operator left, Operator right) => left.Precedence >= right.Precedence;
        public static bool operator <(Operator left, Operator right) => left.Precedence < right.Precedence;
        public static bool operator <=(Operator left, Operator right) => left.Precedence <= right.Precedence;

        public readonly override string ToString() => $"Operator('{Symbol}')";
    }
}
