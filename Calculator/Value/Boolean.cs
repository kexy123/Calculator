using System.Diagnostics.CodeAnalysis;

namespace Core.Value
{
    [method: SetsRequiredMembers]
    public struct Boolean(bool value, string assignedVariable = "") : IValue
    {
        public required bool Value = value;
        public required string AssignedVariable = assignedVariable;

        public readonly ValueType Type => ValueType.Number;
        readonly object IValue.Value => Value;
        readonly string IValue.AssignedVariable => AssignedVariable;

        public readonly override string ToString() => Value ? "TRUE" : "FALSE";

        public static implicit operator bool(Boolean number) => number.Value;

        [SetsRequiredMembers]
        public Boolean(IValue value) : this(value is Boolean boolean ? boolean.Value : throw new ArgumentException(value + " is not a Boolean."), "") { }
        [SetsRequiredMembers]
        public Boolean(object value) : this(value is Boolean boolean ? boolean.Value : value != null) { }
    }
}
