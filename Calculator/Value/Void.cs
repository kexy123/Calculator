namespace Core.Value
{
    public struct VoidToken : IValue
    {
        public void SetAsVariable(string name) => throw new InvalidOperationException("Cannot assign variable to Void()");

        public readonly ValueType Type => ValueType.Ignore;
        readonly object IValue.Value => throw new InvalidValueException("Void() should not get its value");
        readonly string IValue.AssignedVariable => throw new InvalidOperationException("Void() should not get its assigned variable");

        public readonly override string ToString() => "Void()";

        public readonly IValue Clone() => new VoidToken();
    }
}
