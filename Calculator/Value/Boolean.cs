using System.Diagnostics.CodeAnalysis;

namespace Core.Value
{
    [method: SetsRequiredMembers]
    public struct BooleanToken(bool value, string assignedVariable = "") : IValue
    {
        public required bool Value = value;
        public required string AssignedVariable = assignedVariable;

        public readonly ValueType Type => ValueType.Number;
        readonly object IValue.Value => Value;
        readonly string IValue.AssignedVariable => AssignedVariable;

        public void SetAsVariable(string name) => AssignedVariable = name;

        public readonly override string ToString() => Value ? "TRUE" : "FALSE";

        public static implicit operator bool(BooleanToken number) => number.Value;

        [SetsRequiredMembers]
        public BooleanToken(IValue value) : this(value is BooleanToken boolean ? boolean.Value : throw new InvalidValueException(value + " is not a Boolean"), "") { }
        [SetsRequiredMembers]
        public BooleanToken(object value) : this(value is BooleanToken boolean ? boolean.Value : value != null) { }

        public readonly IValue Clone()
        {
            return new BooleanToken(Value, AssignedVariable);
        }
    }
}
