using Core.Common;
using Core.Expression;
using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Operation;
using Core.Value;
using Core.Variable;
using System.Diagnostics.CodeAnalysis;

namespace Core.AssetParsers
{
    [Flags]
    file enum ExpectedForm
    {
        Operand = 1,
        Implicit = 2,
        Operator = 4
    }

    [method: SetsRequiredMembers]
    file struct BracketEntry(Operator closingBracket, Function? function = null)
    {
        public required Operator ClosingBracket = closingBracket;

        readonly int Parameters = 0;
        public readonly Function? AppliedFunction = function;

        /// <summary>
        /// Validates the current function and parameter configuration, throwing an exception if the configuration is invalid.
        /// </summary>
        /// <remarks>Call this method to ensure that the function and its parameters are in a valid state before proceeding with further operations. This method does not return a value; it throws an exception if validation fails.</remarks>
        /// <exception cref="MathArgumentException">Thrown when the number of parameters does not match the expected count for the applied function.</exception>
        public readonly void TryInvalid()
        {
            if (AppliedFunction == null && Parameters != 1) throw new MathArgumentException("Expected 1 parameter in bracket, got " + Parameters);

            uint count = AppliedFunction!.Value.ParameterCount;
            if (Parameters != count) throw new MathArgumentException($"Expected {count} parameter(s) in function, got {Parameters}");
        }

        /// <summary>
        /// Attempts to add a function token to the specified output list if a function is applied.
        /// </summary>
        /// <param name="output">The list to which the function token will be added if applicable.</param>
        public readonly void TryPushInto(List<Token> output)
        {
            TryInvalid();
            if (AppliedFunction is Function function) output.Add(new(TokenType.Function, function.Name, new Number(function.ParameterCount)));
        }

        public static implicit operator BracketEntry(Operator closingBracket) => new(closingBracket);
    }

    file record StateFields
    {
        public static State Fields = new(new() {
            { "Expected", ExpectedForm.Operand },
            { "BracketStack", new Stack<BracketEntry>() },
        });
    }

    public class ArithmeticParser : IParser
    {
        public readonly List<Token> Output = [];
        List<Token> IParser.Output => Output;

        private Stack<Operator> shuntingStack = default!;
        private CalculatorContext calculatorContext = default!;

        private State state;

        private void PopAndPushOperators(Operator operatorObject, Operator? ending = null)
        {
            bool isRightToLeft = operatorObject.ContainsProperty(OperatorProperty.RightToLeft);
            while (true)
            {
                if (shuntingStack.Count == 0) break;
                Operator last = shuntingStack.Last();
                if (last < operatorObject || isRightToLeft && last == operatorObject) break;
                Output.Add(new(TokenType.Operator, shuntingStack.Pop().Symbol));
            }

            if (ending is Operator endOperator && (shuntingStack.Count == 0 || shuntingStack.Last() != endOperator)) throw new OperatorFormatException($"Expected {endOperator}, got {shuntingStack.Last()}");
        }

        private void PushOperator(Token token)
        {
            ExpectedForm newExpected = ExpectedForm.Operand;
            ExpectedForm expected = state.Get<ExpectedForm>("Expected");
            Stack<BracketEntry> bracketStack = state.Get<Stack<BracketEntry>>("BracketStack");

            Operator operatorObject = calculatorContext.DetermineOperationFromString(token.Source);
            if (operatorObject.ContainsProperty(OperatorProperty.ClosedBracket))
            {
                if (expected.HasFlag(ExpectedForm.Operator)) throw new OperatorFormatException($"Expected operator, got {operatorObject}");
                PopAndPushOperators(operatorObject, operatorObject.Opposite!);
                bracketStack.Pop().TryPushInto(Output);
                return;
            }
            else if (operatorObject.ContainsProperty(OperatorProperty.Bracket))
            {
                if (expected.HasFlag(ExpectedForm.Operand)) throw new OperatorFormatException($"Expected operand, got {operatorObject}");
                // TODO: Add function before it to the Bracket Stack if it exists.
                bracketStack.Push(operatorObject.Opposite!);
                shuntingStack.Push(operatorObject);
                state.Set("Expected", newExpected);
                return;
            }

            if (operatorObject.ContainsProperty(OperatorProperty.UnaryPotential))
            {
                //if (operatorObject.ContainsProperty(OperatorProperty.UnaryRight)) // TODO: Make this functional.
                if (expected.HasFlag(ExpectedForm.Operand))
                {
                    operatorObject = operatorObject.Opposite!;
                    newExpected = ExpectedForm.Operand;
                }
            }
            else
            {
                if (expected.HasFlag(ExpectedForm.Operand)) throw new OperatorFormatException($"Expected operand, got {operatorObject}");
            }
            PopAndPushOperators(operatorObject);
            shuntingStack.Push(operatorObject);
            state.Set("Expected", newExpected);
        }

        private void PushOperand(Token token)
        {
            ExpectedForm expected = state.Get<ExpectedForm>("Expected");
            if (!expected.HasFlag(ExpectedForm.Operand)) throw new OperatorFormatException($"Expected operand, got {token}");
            Output.Add(token);
            state.Set("Expected", ExpectedForm.Operator); // TODO: Implement ExpectedForm.Implicit for numbers and variables.
        }

        public void Parse(in Token[] tokens, CalculatorContext context)
        {
            Output.Clear();

            state = StateFields.Fields;
            state.Set("Context", context);

            calculatorContext = context;
            shuntingStack = new();

            foreach (Token token in tokens)
            {
                if (token.ContainsProperty(TokenType.Operator)) PushOperator(token);
                if (token.ContainsProperty(TokenType.Operand)) PushOperand(token);
            }

            while (shuntingStack.Count > 0)
            {
                Operator last = shuntingStack.Pop();
                if (last.ContainsProperty(OperatorProperty.Bracket)) throw new OperatorFormatException("Mismatched brackets.");
                Output.Add(new(TokenType.Operator, last.Symbol));
            }
        }
    }
}
