namespace Core.Value
{
    public enum ValueType
    {
        Number,
        Variable,
        Array,
        Expression,
    }

    public interface IValue {
        public object Value { get; }
        public string AssignedVariable { get; }
        public ValueType Type { get; }
    }

    public static class Value {
        public static Number ToNumber(IValue obj) => obj is Number number ? number : throw new InvalidValueException(obj + " is not a Number");
    }
}
