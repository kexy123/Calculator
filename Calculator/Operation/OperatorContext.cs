namespace Calculator.Operation
{
    public interface IOperatorContext
    {
        public Operator[] Operators { get; }

        /// <summary>
        /// Determines the operation given a string of combined operators.
        /// </summary>
        /// <param name="source">The string to look at.</param>
        /// <returns>An Operator derived from that string.</returns>
        public Operator DetermineOperationFromString(string source);
    }
}
