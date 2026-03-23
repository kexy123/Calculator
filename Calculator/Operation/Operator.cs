using System.Diagnostics.CodeAnalysis;
using Core.Expression;
using Core.Value;

namespace Core.Operation
{
    using Form = Func<IValue, IValue, CalculatorContext, IValue>;

    [Flags]
    public enum OperatorProperty
    {
        Regular = 0,
        RightToLeft = 1,

        UnaryPotential = 2,
        Unary = 4,
        UnaryRight = 12,

        Transitive = 17,

        Bracket = 32,
        ClosedBracket = 96,

        Ignore = 128,

        Separator = 256,
    }

    [method: SetsRequiredMembers]
    public class Operator(string symbol, OperatorProperty properties, sbyte precedence, Form function, Operator? opposite = null)
    {
        public required string Symbol = symbol;
        public required OperatorProperty Properties = properties;
        public required sbyte Precedence = precedence;
        public required Form Execute = function;

        public Operator? Opposite = opposite;

        /// <summary>
        /// Determines whether the specified property or set of properties is present.
        /// </summary>
        /// <param name="property">The property or combination of properties to check for within the Operator.</param>
        /// <returns>true if all specified properties are present, and false if not.</returns>
        public bool ContainsProperty(OperatorProperty property) => Properties.HasFlag(property);

        public override bool Equals(object? obj) => obj is Operator operatorObject && Symbol == operatorObject.Symbol;
        public override int GetHashCode() => Symbol.GetHashCode();

        /// <summary>
        /// Determines whether two Operators are equal by their symbols.
        /// </summary>
        /// <param name="left">The operator on the left to check.</param>
        /// <param name="right">The operator on the right to check.</param>
        /// <returns>true if they have the same symbol, and false if not.</returns>
        public static bool operator ==(Operator? left, Operator? right)
        {
            if (left is null || right is null) return false;
            return left.Symbol == right.Symbol;
        }
        public static bool operator !=(Operator? left, Operator? right) => !(left == right);

        public static bool operator >(Operator left, Operator right) => left.Precedence > right.Precedence;
        public static bool operator >=(Operator left, Operator right) => left.Precedence >= right.Precedence;
        public static bool operator <(Operator left, Operator right) => left.Precedence < right.Precedence;
        public static bool operator <=(Operator left, Operator right) => left.Precedence <= right.Precedence;

        public override string ToString() => $"Operator('{Symbol}')";
    }
}
