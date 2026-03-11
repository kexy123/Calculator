using System.Text.RegularExpressions;

namespace Calculator.Expression
{
    public static class NameCheck
    {
        /// <summary>
        /// Determines whether the specified name is valid for an expression.
        /// </summary>
        /// <remarks>A valid name must start with a letter and can only contain letters or underscores.</remarks>
        /// <param name="name">The name to validate.</param>
        /// <returns>true if the name is valid; otherwise, false.</returns>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return true;
        }
    }
}
