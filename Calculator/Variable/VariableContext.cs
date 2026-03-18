using Core.Value;

namespace Core.Variable
{
    public interface IVariableContext
    {
        public Dictionary<string, Function> Functions { get; }
        public Dictionary<string, IValue> Variables { get; }

        /// <summary>
        /// Assigns the given function into the context.
        /// </summary>
        /// <param name="function">The function to implement.</param>
        public void AssignFunction(Function function);

        /// <summary>
        /// Assigns the given variable into the context.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The variable's value to implement.</param>
        public void AssignVariable(string name, IValue value);

        /// <summary>
        /// Attempts to get the variable given the string sequence.
        /// </summary>
        /// <param name="name">The string sequence to get the variable from.</param>
        /// <returns>The IValue tied to the variable name.</returns>
        public IValue GetVariableFromName(string name);
    }
}
