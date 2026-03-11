using System.Text.RegularExpressions;

namespace Calculator.Variable
{
    public static class NameCheck
    {
        public static readonly Regex NamePattern = new("^[a-zA-Z][a-zA-Z_]*$", RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified name is valid for an expression.
        /// </summary>
        /// <remarks>A valid name must start with a letter and can only contain letters or underscores.</remarks>
        /// <param name="name">The name to validate.</param>
        /// <returns>true if the name is valid; otherwise, false.</returns>
        public static bool IsValidName(string name) => string.IsNullOrEmpty(name) || !NamePattern.IsMatch(name);

        public static void ThrowOnInvalidName(string name)
        {
            if (!IsValidName(name)) throw new ArgumentException($"'{name}' must be a valid variable/function name. It can only contain letters and underscores, and cannot start with an underscore.");
        }
    }
}
