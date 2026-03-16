using System.Diagnostics.CodeAnalysis;

namespace Core.Value
{
    [method: SetsRequiredMembers]
    public struct Number(double value, string assignedVariable = "") : IValue
    {
        public required double Value = value;
        public required string AssignedVariable = assignedVariable;

        public readonly ValueType Type => ValueType.Number;
        readonly object IValue.Value => Value;
        readonly string IValue.AssignedVariable => AssignedVariable;

        public readonly override string ToString() => Convert.ToString(Value);
        //public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Number number && number.Value == Value;

        //public static implicit operator decimal(Number number) => number.Value;
        public static implicit operator double(Number number) => number.Value;

        [SetsRequiredMembers]
        public Number(IValue value) : this(value is Number number ? number.Value : throw new InvalidValueException(value + " is not a number"), "") { }

        public static implicit operator Number(sbyte value) => new(value);
        public static implicit operator Number(byte value) => new(value);
        public static implicit operator Number(short value) => new(value);
        public static implicit operator Number(ushort value) => new(value);
        public static implicit operator Number(int value) => new(value);
        public static implicit operator Number(nint value) => new(value);
        public static implicit operator Number(uint value) => new(value);
        public static implicit operator Number(nuint value) => new(value);
        public static implicit operator Number(long value) => new(value);
        public static implicit operator Number(ulong value) => new(value);

        public static implicit operator Number(float value) => new(value);
        public static implicit operator Number(double value) => new(value);
        //public static implicit operator Number(decimal value) => new(value);

        public static Number operator +(Number a, Number b) => a.Value + b.Value;
        public static Number operator -(Number a, Number b) => a.Value - b.Value;
        public static Number operator *(Number a, Number b) => a.Value * b.Value;
        public static Number operator /(Number a, Number b) => a.Value / b.Value;
        public static Number operator %(Number a, Number b) => a.Value % b.Value;

        public static implicit operator Number(Boolean value) => new(value == true ? 1 : 0);
    }
}
