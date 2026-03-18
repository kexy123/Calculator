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

        /// <summary>
        /// Sets the value of a variable identified by the specified name.
        /// </summary>
        /// <param name="name">The name of the variable to set. Cannot be null or empty.</param>
        public void SetAsVariable(string name);
    }

    public static class Value {
        public static Number ToNumber(IValue obj) => obj is Number number ? number : throw new InvalidValueException(obj + " is not a Number");
    }
}
