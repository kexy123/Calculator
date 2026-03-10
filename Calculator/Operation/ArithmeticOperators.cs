using Calculator.Value;

namespace Calculator.Operation
{
    public record class ArithmeticOperators
    {
        public static readonly Operator<Number, Number> Addition = new("+", OperatorProperty.Regular, 4, (a, b) => a + b);
        public static readonly Operator<Number, Number> Subtraction = new("-", OperatorProperty.Regular, 4, (a, b) => a - b);
        public static readonly Operator<Number, Number> Multiplication = new("*", OperatorProperty.Regular, 8, (a, b) => a * b);
        public static readonly Operator<Number, Number> Division = new("/", OperatorProperty.Regular, 8, (a, b) => a / b);
        public static readonly Operator<Number, Number> Modulo = new("%", OperatorProperty.Regular, 8, (a, b) => a % b);
        public static readonly Operator<Number, Number> Exponentiation = new("^", OperatorProperty.RightToLeft, 16, (a, b) => Math.Pow(a, b));
    }
}
