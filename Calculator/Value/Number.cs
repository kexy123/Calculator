using System.Diagnostics.CodeAnalysis;

namespace Core.Value
{
    [method: SetsRequiredMembers]
    public struct NumberToken(double value, string assignedVariable = "") : IValue
    {
        public required double Value = value;
        public required string AssignedVariable = assignedVariable;

        public readonly ValueType Type => ValueType.Number;
        readonly object IValue.Value => Value;
        readonly string IValue.AssignedVariable => AssignedVariable;

        public void SetAsVariable(string name) => AssignedVariable = name;

        public readonly override string ToString() => Convert.ToString(Value);
        //public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Number number && number.Value == Value;

        //public static implicit operator decimal(Number number) => number.Value;
        public static implicit operator double(NumberToken number) => number.Value;

        [SetsRequiredMembers]
        public NumberToken(IValue value) : this(value is NumberToken number ? number.Value : throw new InvalidValueException(value + " is not a number"), "") { }

        public static implicit operator NumberToken(sbyte value) => new(value);
        public static implicit operator NumberToken(byte value) => new(value);
        public static implicit operator NumberToken(short value) => new(value);
        public static implicit operator NumberToken(ushort value) => new(value);
        public static implicit operator NumberToken(int value) => new(value);
        public static implicit operator NumberToken(nint value) => new(value);
        public static implicit operator NumberToken(uint value) => new(value);
        public static implicit operator NumberToken(nuint value) => new(value);
        public static implicit operator NumberToken(long value) => new(value);
        public static implicit operator NumberToken(ulong value) => new(value);

        public static implicit operator NumberToken(float value) => new(value);
        public static implicit operator NumberToken(double value) => new(value);
        //public static implicit operator Number(decimal value) => new(value);

        public static NumberToken operator +(NumberToken a, NumberToken b) => a.Value + b.Value;
        public static NumberToken operator -(NumberToken a, NumberToken b) => a.Value - b.Value;
        public static NumberToken operator *(NumberToken a, NumberToken b) => a.Value * b.Value;
        public static NumberToken operator /(NumberToken a, NumberToken b) => a.Value / b.Value;
        public static NumberToken operator %(NumberToken a, NumberToken b) => a.Value % b.Value;

        public static implicit operator NumberToken(BooleanToken value) => new(value == true ? 1 : 0);
    }
}
