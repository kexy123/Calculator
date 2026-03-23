using System.Diagnostics.CodeAnalysis;

namespace Core.Value
{
    [method: SetsRequiredMembers]
    public struct NothingToken(string assignedVariable = "") : IValue
    {
        public string AssignedVariable = assignedVariable;

        public void SetAsVariable(string name) => AssignedVariable = name;

        public readonly ValueType Type => ValueType.Number;
        readonly object IValue.Value
        {
            get
            {
                if (AssignedVariable == "") throw new InvalidValueException("Nothing() should not get its value");
                else throw new InvalidValueException($"The variable '{AssignedVariable}' does not exist in the current context");
            }
        }
        readonly string IValue.AssignedVariable => AssignedVariable;

        public readonly override string ToString() => "Nothing()";

        public readonly IValue Clone()
        {
            return new NothingToken(AssignedVariable);
        }
    }
}
