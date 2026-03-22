using Core.AssetParsers;
using Core.Common;
using Core.Expression;
using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Operation;
using Core.Value;
using Core.Variable;
using System.Text.RegularExpressions;

namespace Core.AssetContexts
{
    using TokenFunction = Action<Tokenizer, Match>;

    public class Arithmetic : ICalculatorContext
    {
        TokenPattern[] ICalculatorContext.TokenPatterns => TokenPatterns.ExpressionPatterns;
        Operator[] ICalculatorContext.Operators => [
            Operators.OpenBracket, Operators.ClosingBracket,
            Operators.Comma,
            Operators.Assign,
            Operators.UnaryAddition, Operators.Addition, Operators.UnarySubtraction, Operators.Subtraction, Operators.Multiplication, Operators.Division, Operators.Modulo, Operators.Exponentiation
        ];
        IParser ICalculatorContext.Parser => new ArithmeticParser();
    }

    // Arithmetic tokens
    file record Tokens
    {
        public static TokenFunction DumpNumber = (tokenizer, match) =>
        {
            string number = match.Value;
            tokenizer.AddToken(new(TokenType.Number, number, new NumberToken(Convert.ToDouble(number))));
            tokenizer.Index += number.Length;
        };

        public static TokenFunction DumpIdentifier = (tokenizer, match) =>
        {
            tokenizer.Context.GetFunctionFromName(match.Value, out string result);
            if (result != "")
            {
                tokenizer.AddToken(new(TokenType.Function, result));
                tokenizer.Index += result.Length;
            }
            else
            {
                IValue value = tokenizer.Context.GetVariableFromName(match.Value);
                tokenizer.AddToken(new(TokenType.Variable, match.Value, value));
                tokenizer.Index += value.AssignedVariable.Length;
            }
        };
        
        public static TokenFunction DumpOperation = (tokenizer, match) =>
        {
            string operation = match.Value;
            Operator operatorObject = tokenizer.Context.DetermineOperationFromString(operation);
            tokenizer.AddToken(new(TokenType.Operator, operatorObject.Symbol));
            tokenizer.Index += operatorObject.Symbol.Length;
        };

        public static TokenFunction DumpWhitespace = (tokenizer, match) => tokenizer.Index += match.Value.Length;
    }

    file record TokenPatterns
    {
        public static TokenPattern[] ExpressionPatterns = [
            new(@"[\x00-\x1F\x7F\s ]+", Tokens.DumpWhitespace),

            new(@"[a-zA-Z][_a-zA-Z]*", Tokens.DumpIdentifier),

            new(@"\d*\.\d*", Tokens.DumpNumber),
            new(@"\d+", Tokens.DumpNumber),

            new(@"[-+*/^%()|<>=,]+", Tokens.DumpOperation),
        ];
    }

    // Operators
    file record OperatorFunctions
    {
        public static IValue DoNothing(IValue _, IValue __, CalculatorContext ___) => throw new InvalidOperationException("This should not run");

        public static IValue Assign(IValue a, IValue b, CalculatorContext c)
        {
            string name = a.AssignedVariable;
            if (name == "") throw new InvalidValueException(a + " is not a variable name");
            c.AssignVariable(name, b);
            return b;
        }

        public static IValue UnaryAddition(IValue _, IValue b, CalculatorContext __) => new NumberToken(b);
        public static IValue Addition(IValue a, IValue b, CalculatorContext _) => new NumberToken(a) + new NumberToken(b);
        public static IValue UnarySubtraction(IValue _, IValue b, CalculatorContext __) => 0 - new NumberToken(b);
        public static IValue Subtraction(IValue a, IValue b, CalculatorContext _) => new NumberToken(a) - new NumberToken(b);
        public static IValue Multiplication(IValue a, IValue b, CalculatorContext _) => new NumberToken(a) * new NumberToken(b);
        public static IValue Division(IValue a, IValue b, CalculatorContext _) => new NumberToken(a) / new NumberToken(b);
        public static IValue Modulo(IValue a, IValue b, CalculatorContext _) => new NumberToken(a) % new NumberToken(b);
        public static IValue Exponentiation(IValue a, IValue b, CalculatorContext _) => new NumberToken(Math.Pow(new NumberToken(a), new NumberToken(b)));
    }

    file record Operators
    {
        public static readonly Operator OpenBracket = new("(", OperatorProperty.Bracket, 0, OperatorFunctions.DoNothing);
        public static readonly Operator ClosingBracket = new(")", OperatorProperty.ClosedBracket, 0, OperatorFunctions.DoNothing);
        public static readonly Operator Comma = new(",", OperatorProperty.Separator, 0, OperatorFunctions.DoNothing);

        public static readonly Operator Assign = new("->", OperatorProperty.Regular, 1, OperatorFunctions.Assign);



        public static readonly Operator UnaryAddition = new("1+", OperatorProperty.Unary | OperatorProperty.RightToLeft, 3, OperatorFunctions.UnaryAddition);
        public static readonly Operator Addition = new("+", OperatorProperty.UnaryPotential, 3, OperatorFunctions.Addition, UnaryAddition);
        public static readonly Operator UnarySubtraction = new("1-", OperatorProperty.Unary | OperatorProperty.RightToLeft, 3, OperatorFunctions.UnarySubtraction);
        public static readonly Operator Subtraction = new("-", OperatorProperty.UnaryPotential, 3, OperatorFunctions.Subtraction, UnarySubtraction);

        public static readonly Operator Multiplication = new("*", OperatorProperty.Regular, 7, OperatorFunctions.Multiplication);
        public static readonly Operator Division = new("/", OperatorProperty.Regular, 7, OperatorFunctions.Division);
        public static readonly Operator Modulo = new("%", OperatorProperty.Regular, 7, OperatorFunctions.Modulo);

        public static readonly Operator Exponentiation = new("^", OperatorProperty.RightToLeft, 15, OperatorFunctions.Exponentiation);

        static Operators()
        {
            OpenBracket.Opposite = ClosingBracket;
            ClosingBracket.Opposite = OpenBracket;
        }
    }
}
