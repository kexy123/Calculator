using Core.AssetParsers;
using Core.Common;
using Core.Expression;
using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Operation;
using Core.Value;
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
            Operators.ExclamationMark, Operators.UnaryNot,
            Operators.Assign,
            Operators.And,
            Operators.IsEqualTo, Operators.IsNotEqualTo, Operators.IsLessThan, Operators.IsGreaterThan, Operators.IsLEQTo, Operators.IsGEQTo,
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
            tokenizer.AddToken(new(TokenProperty.Number, number, new NumberToken(Convert.ToDouble(number))));
            tokenizer.Index += number.Length;
        };

        public static TokenFunction DumpIdentifier = (tokenizer, match) =>
        {
            tokenizer.Context.GetFunctionFromName(match.Value, out string result);
            if (result != "")
            {
                tokenizer.AddToken(new(TokenProperty.Function, result));
                tokenizer.Index += result.Length;
            }
            else
            {
                IValue value = tokenizer.Context.GetVariableFromName(match.Value);
                tokenizer.AddToken(new(TokenProperty.Variable, value.AssignedVariable, value));
                tokenizer.Index += value.AssignedVariable.Length;
            }
        };
        
        public static TokenFunction DumpOperation = (tokenizer, match) =>
        {
            string operation = match.Value;
            Operator operatorObject = tokenizer.Context.DetermineOperationFromString(operation);
            tokenizer.AddToken(new(TokenProperty.Operator, operatorObject.Symbol));
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

            new(@"[-+*/^%(){}|<>=,&!]+", Tokens.DumpOperation),
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

        public static IValue IsEqualTo(IValue a, IValue b, CalculatorContext c)
        {
            if (a is NothingToken)
            {
                string name = a.AssignedVariable;
                if (name == "") throw new InvalidValueException(a + " is not a variable name");
                c.AssignVariable(name, b);
                return new BooleanToken(true, note: $"Assigned '{name}' to value");
            }
            return new BooleanToken(object.Equals(a.Value, b.Value));
        }

        public static IValue IsNotEqualTo(IValue a, IValue b, CalculatorContext _) => new BooleanToken(!object.Equals(a.Value, b.Value));
        public static IValue IsGreaterThan(IValue a, IValue b, CalculatorContext _) => new BooleanToken(((NumberToken)a).Value > ((NumberToken)b).Value);
        public static IValue IsLessThan(IValue a, IValue b, CalculatorContext _) => new BooleanToken(((NumberToken)a).Value < ((NumberToken)b).Value);
        public static IValue IsGEQTo(IValue a, IValue b, CalculatorContext _) => new BooleanToken(((NumberToken)a).Value >= ((NumberToken)b).Value);
        public static IValue IsLEQTo(IValue a, IValue b, CalculatorContext _) => new BooleanToken(((NumberToken)a).Value <= ((NumberToken)b).Value);

        public static IValue Not(IValue _, IValue b, CalculatorContext __) => new BooleanToken(!((BooleanToken)b).Value);
        public static IValue And(IValue a, IValue b, CalculatorContext _) => new BooleanToken(((BooleanToken)a).Value && ((BooleanToken)b).Value);

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
        public static readonly Operator OpenPiecewiseBracket = new("{", OperatorProperty.Bracket, 0, OperatorFunctions.DoNothing);
        public static readonly Operator ClosingPiecewiseBracket = new("}", OperatorProperty.ClosedBracket, 0, OperatorFunctions.DoNothing);

        public static readonly Operator Comma = new(",", OperatorProperty.Separator, 0, OperatorFunctions.DoNothing);

        //public static readonly Operator Colon = new(":", OperatorProperty.Separator, 0, OperatorFunctions.DoNothing);

        public static readonly Operator Assign = new("->", OperatorProperty.Regular, 1, OperatorFunctions.Assign);

        public static readonly Operator And = new("&&", OperatorProperty.Regular, 3, OperatorFunctions.And);
        public static readonly Operator UnaryNot = new("u!", OperatorProperty.Unary | OperatorProperty.RightToLeft, 3, OperatorFunctions.Not);
        public static readonly Operator ExclamationMark = new("!", OperatorProperty.UnaryPotential, 0, OperatorFunctions.DoNothing, UnaryNot);

        public static readonly Operator IsEqualTo = new("=", OperatorProperty.Transitive, 7, OperatorFunctions.IsEqualTo);
        public static readonly Operator IsNotEqualTo = new("!=", OperatorProperty.Transitive, 7, OperatorFunctions.IsNotEqualTo);
        public static readonly Operator IsGreaterThan = new(">", OperatorProperty.Transitive, 7, OperatorFunctions.IsGreaterThan);
        public static readonly Operator IsLessThan = new("<", OperatorProperty.Transitive, 7, OperatorFunctions.IsLessThan);
        public static readonly Operator IsGEQTo = new(">=", OperatorProperty.Transitive, 7, OperatorFunctions.IsGEQTo);
        public static readonly Operator IsLEQTo = new("<=", OperatorProperty.Transitive, 7, OperatorFunctions.IsLEQTo);

        public static readonly Operator UnaryAddition = new("u+", OperatorProperty.Unary | OperatorProperty.RightToLeft | OperatorProperty.Ignore, 15, OperatorFunctions.UnaryAddition);
        public static readonly Operator Addition = new("+", OperatorProperty.UnaryPotential, 15, OperatorFunctions.Addition, UnaryAddition);
        public static readonly Operator UnarySubtraction = new("u-", OperatorProperty.Unary | OperatorProperty.RightToLeft, 15, OperatorFunctions.UnarySubtraction);
        public static readonly Operator Subtraction = new("-", OperatorProperty.UnaryPotential, 15, OperatorFunctions.Subtraction, UnarySubtraction);

        public static readonly Operator Multiplication = new("*", OperatorProperty.Regular, 31, OperatorFunctions.Multiplication);
        public static readonly Operator Division = new("/", OperatorProperty.Regular, 31, OperatorFunctions.Division);
        public static readonly Operator Modulo = new("%", OperatorProperty.Regular, 31, OperatorFunctions.Modulo);

        public static readonly Operator Exponentiation = new("^", OperatorProperty.RightToLeft, 63, OperatorFunctions.Exponentiation);

        static Operators()
        {
            OpenBracket.Opposite = ClosingBracket;
            ClosingBracket.Opposite = OpenBracket;

            OpenPiecewiseBracket.Opposite = ClosingPiecewiseBracket;
            ClosingPiecewiseBracket.Opposite = OpenPiecewiseBracket;
        }
    }
}
