using Calculator.Expression.Token;
using Calculator.Operation;
using Calculator.Value;
using System.Text.RegularExpressions;

namespace Calculator.Contexts
{
    using TokenFunction = Action<Tokenizer, Match>;

    // Arithmetic tokens
    public record ArithmeticTokens
    {
        public static TokenFunction DumpNumber = (tokenizer, match) =>
        {
            string number = match.Value;
            tokenizer.AddToken(new(TokenType.Number, number));
            tokenizer.Index += number.Length;
        };

        public static TokenFunction DumpWhitespace = (tokenizer, match) => tokenizer.Index += match.Value.Length;
    }

    public partial record TokenPatterns
    {
        public static TokenPattern[] ExpressionPatterns = [
            new(@"[\x00-\x1F\x7F\s ]+", ArithmeticTokens.DumpWhitespace),

            new(@"\d*\.\d*", ArithmeticTokens.DumpNumber),
            new(@"\d+", ArithmeticTokens.DumpNumber),

            //new(@"[-+*/^%]+", ArithmeticTokens.DumpOperation),
        ];
    }

    // Operators
    public partial record class OperatorFunctions
    {
        public static IValue Addition(IValue a, IValue b) => new Number(new Number(a) + new Number(b));
        public static IValue Subtraction(IValue a, IValue b) => new Number(new Number(a) - new Number(b));
        public static IValue Multiplication(IValue a, IValue b) => new Number(new Number(a) * new Number(b));
        public static IValue Division(IValue a, IValue b) => new Number(new Number(a) / new Number(b));
        public static IValue Modulo(IValue a, IValue b) => new Number(new Number(a) % new Number(b));
        public static IValue Exponentiation(IValue a, IValue b) => new Number(Math.Pow(new Number(a), new Number(b)));
    }

    public partial record class Operators
    {
        public static readonly Operator Addition = new("+", OperatorProperty.Regular, 4, OperatorFunctions.Addition);
        public static readonly Operator Subtraction = new("-", OperatorProperty.Regular, 4, OperatorFunctions.Subtraction);

        public static readonly Operator Multiplication = new("*", OperatorProperty.Regular, 8, OperatorFunctions.Multiplication);
        public static readonly Operator Division = new("/", OperatorProperty.Regular, 8, OperatorFunctions.Division);
        public static readonly Operator Modulo = new("%", OperatorProperty.Regular, 8, OperatorFunctions.Modulo);

        public static readonly Operator Exponentiation = new("^", OperatorProperty.RightToLeft, 16, OperatorFunctions.Exponentiation);
    }
}
