using Core.Variable;
using System.Diagnostics.CodeAnalysis;

namespace Core.Value
{
    [method: SetsRequiredMembers]
    public struct FunctionToken(Function value, string assignedVariable = "") : IValue
    {
        public required Function Value = value;
        public required string AssignedVariable = assignedVariable;

        public readonly ValueType Type => ValueType.Function;
        readonly object IValue.Value => Value;
        readonly string IValue.AssignedVariable => AssignedVariable;

        public void SetAsVariable(string name) => AssignedVariable = name;

        public readonly override string ToString() => Convert.ToString(Value)!;

        public readonly IValue Clone()
        {
            return new FunctionToken(Value, AssignedVariable);
        }
    }
}
