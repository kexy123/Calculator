global using Core.Common;

namespace Core.Common
{
    public class OperatorFormatException(string message) : Exception(message);
    public class MathArgumentException(string message) : Exception(message);
    public class FaultyExpressionException(string message) : Exception(message);

    public class InvalidValueException(string message) : Exception(message);
    public class InvalidNameException(string message) : Exception(message);
    public class InvalidTokenException(string message) : Exception(message);
}
