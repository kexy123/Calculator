using Calculator.Value;

namespace Calculator.Variable
{
    public interface IVariableContext
    {
        public Dictionary<string, Function> Functions { get; }
        public Dictionary<string, IValue> Variables { get; }

        /// <summary>
        /// Implements the given function into the context.
        /// </summary>
        /// <param name="function">The function to implement.</param>
        public void ImplementFunction(Function function);

        /// <summary>
        /// Implements the given variable into the context.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The variable's value to implement.</param>
        public void ImplementVariable(string name, IValue value);
    }
}
