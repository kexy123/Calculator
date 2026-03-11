using Calculator.Value;

namespace Calculator.Operation
{
    public record class ArithmeticOpFunctions
    {
        public static IValue Addition(IValue a, IValue b) => new Number(new Number(a) + new Number(b));
        public static IValue Subtraction(IValue a, IValue b) => new Number(new Number(a) - new Number(b));
        public static IValue Multiplication(IValue a, IValue b) => new Number(new Number(a) * new Number(b));
        public static IValue Division(IValue a, IValue b) => new Number(new Number(a) / new Number(b));
        public static IValue Modulo(IValue a, IValue b) => new Number(new Number(a) % new Number(b));
        public static IValue Exponentiation(IValue a, IValue b) => new Number(Math.Pow(new Number(a), new Number(b)));
    }

    public record class ArithmeticOperators
    {
        public static readonly Operator Addition = new("+", OperatorProperty.Regular, 4, ArithmeticOpFunctions.Addition);
        public static readonly Operator Subtraction = new("-", OperatorProperty.Regular, 4, ArithmeticOpFunctions.Subtraction);

        public static readonly Operator Multiplication = new("*", OperatorProperty.Regular, 8, ArithmeticOpFunctions.Multiplication);
        public static readonly Operator Division = new("/", OperatorProperty.Regular, 8, ArithmeticOpFunctions.Division);
        public static readonly Operator Modulo = new("%", OperatorProperty.Regular, 8, ArithmeticOpFunctions.Modulo);

        public static readonly Operator Exponentiation = new("^", OperatorProperty.RightToLeft, 16, ArithmeticOpFunctions.Exponentiation);
    }
}
