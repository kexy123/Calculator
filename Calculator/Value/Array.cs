using System.Diagnostics.CodeAnalysis;

namespace Core.Value
{
    [method: SetsRequiredMembers]
    public struct ArrayToken(IValue[] values, string assignedVariable = "") : IValue
    {
        public required IValue[] Values = values;
        public required string AssignedVariable = assignedVariable;

        public readonly ValueType Type => ValueType.Number;
        readonly object IValue.Value => Values;
        readonly string IValue.AssignedVariable => AssignedVariable;

        public void SetAsVariable(string name) => AssignedVariable = name;

        /// <summary>
        /// Gets the value in the array from the key.
        /// </summary>
        /// <param name="key">The value assigned to that key.</param>
        public readonly IValue GetValue(int key) => Values[key];

        public readonly override string ToString() => string.Join(", ", Values);

        [SetsRequiredMembers]
        public ArrayToken(IValue value, string assignedVariable = "") : this([value], assignedVariable) { }
    }
}
