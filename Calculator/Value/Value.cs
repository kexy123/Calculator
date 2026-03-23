namespace Core.Value
{
    public enum ValueType
    {
        Number,
        Variable,
        Array,
        Function,
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

        /// <summary>
        /// Clones itself.
        /// </summary>
        /// <remarks>This exists in the event of the requriement to clone a boxed struct, especially under an interface.</remarks>
        /// <returns>Itself.</returns>
        public IValue Clone();
    }

    public static class Value {
        public static NumberToken ToNumber(IValue obj) => obj is NumberToken number ? number : throw new InvalidValueException(obj + " is not a Number");
    }
}
