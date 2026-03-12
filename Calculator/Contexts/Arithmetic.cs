using Calculator.Expression;
using Calculator.Expression.Token;
using Calculator.Operation;
using Calculator.Value;
using System.Text.RegularExpressions;

namespace Calculator.Contexts
{
    using TokenFunction = Action<Tokenizer, Match>;

    public class Arithmetic : ICalculatorContext
    {
        TokenPattern[] ICalculatorContext.TokenPatterns => TokenPatterns.ExpressionPatterns;
        Operator[] ICalculatorContext.Operators => [
            Operators.OpenBracket, Operators.ClosingBracket,
            Operators.Addition, Operators.Subtraction, Operators.Multiplication, Operators.Division, Operators.Modulo, Operators.Exponentiation
        ];
    }

    // Arithmetic tokens
    file record Tokens
    {
        public static TokenFunction DumpNumber = (tokenizer, match) =>
        {
            string number = match.Value;
            tokenizer.AddToken(new(TokenType.Number, number, Convert.ToDouble(number)));
            tokenizer.Index += number.Length;
        };
        
        public static TokenFunction DumpOperation = (tokenizer, match) =>
        {
            string operation = match.Value;
            Operator operatorObject = tokenizer.Context.DetermineOperationFromString(operation);
            tokenizer.AddToken(new(TokenType.Operator, operatorObject.Symbol, operatorObject));
            tokenizer.Index += operatorObject.Symbol.Length;
        };

        public static TokenFunction DumpWhitespace = (tokenizer, match) => tokenizer.Index += match.Value.Length;
    }

    file record TokenPatterns
    {
        public static TokenPattern[] ExpressionPatterns = [
            new(@"[\x00-\x1F\x7F\s ]+", Tokens.DumpWhitespace),

            new(@"\d*\.\d*", Tokens.DumpNumber),
            new(@"\d+", Tokens.DumpNumber),

            new(@"[-+*/^%()|]+", Tokens.DumpOperation),
        ];
    }

    // Operators
    file record OperatorFunctions
    {
        public static IValue DoNothing(IValue _, IValue __) => throw new InvalidOperationException("This should not run.");

        public static IValue Addition(IValue a, IValue b) => new Number(new Number(a) + new Number(b));
        public static IValue Subtraction(IValue a, IValue b) => new Number(new Number(a) - new Number(b));
        public static IValue Multiplication(IValue a, IValue b) => new Number(new Number(a) * new Number(b));
        public static IValue Division(IValue a, IValue b) => new Number(new Number(a) / new Number(b));
        public static IValue Modulo(IValue a, IValue b) => new Number(new Number(a) % new Number(b));
        public static IValue Exponentiation(IValue a, IValue b) => new Number(Math.Pow(new Number(a), new Number(b)));
    }

    file record Operators
    {
        public static readonly Operator OpenBracket = new("(", OperatorProperty.Bracket, 0, OperatorFunctions.DoNothing);
        public static readonly Operator ClosingBracket = new(")", OperatorProperty.Bracket, 127, OperatorFunctions.DoNothing);

        public static readonly Operator Addition = new("+", OperatorProperty.Regular, 4, OperatorFunctions.Addition);
        public static readonly Operator Subtraction = new("-", OperatorProperty.Regular, 4, OperatorFunctions.Subtraction);

        public static readonly Operator Multiplication = new("*", OperatorProperty.Regular, 8, OperatorFunctions.Multiplication);
        public static readonly Operator Division = new("/", OperatorProperty.Regular, 8, OperatorFunctions.Division);
        public static readonly Operator Modulo = new("%", OperatorProperty.Regular, 8, OperatorFunctions.Modulo);

        public static readonly Operator Exponentiation = new("^", OperatorProperty.RightToLeft, 16, OperatorFunctions.Exponentiation);
    }
}
