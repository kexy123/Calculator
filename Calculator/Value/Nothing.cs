using System.Diagnostics.CodeAnalysis;

namespace Core.Value
{
    [method: SetsRequiredMembers]
    public struct Nothing() : IValue
    {
        public readonly ValueType Type => ValueType.Number;
        readonly object IValue.Value => throw new InvalidValueException("Nothing() should not get its value.");
        readonly string IValue.AssignedVariable => throw new InvalidValueException("Nothing() should not get its variable.");

        public readonly override string ToString() => "Nothing";
    }
}
