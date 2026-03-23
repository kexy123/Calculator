using System.Diagnostics.CodeAnalysis;
using Core.Common;
using Core.Expression;
using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Operation;
using Core.Value;
using Core.Variable;

namespace Core.AssetParsers
{
    using FunctionOverloadList = Dictionary<int, Function>;

    [Flags]
    file enum ExpectedForm
    {
        Operand = 1,
        Implicit = 2,
        Operator = 4,
        Parameter = 8
    }

    [method: SetsRequiredMembers]
    file class BracketEntry(Operator closingBracket, string? function = null)
    {
        public required Operator? ClosingBracket = closingBracket;

        public int Parameters = 0;
        public readonly string? AppliedFunction = function;

        /// <summary>
        /// Validates the current function and parameter configuration, throwing an exception if the configuration is invalid.
        /// </summary>
        /// <remarks>Call this method to ensure that the function and its parameters are in a valid state before proceeding with further operations. This method does not return a value; it throws an exception if validation fails.</remarks>
        /// <exception cref="MathArgumentException">Thrown when the number of parameters does not match the expected count for the applied function.</exception>
        public void TryInvalid(CalculatorContext context, out FunctionOverloadList? overloads)
        {
            if (AppliedFunction == null)
            {
                overloads = null;
                if (Parameters == 1) return;
                throw new MathArgumentException("Expected 1 parameter in bracket, got " + Parameters);
            }

            overloads = context.GetFunctionFromName(AppliedFunction, out _);
            context.GetFunction(overloads!, Parameters);
        }

        /// <summary>
        /// Attempts to add a function token to the specified output list if a function is applied.
        /// </summary>
        /// <param name="output">The list to which the function token will be added if applicable.</param>
        public void TryPushInto(CalculatorContext context, List<Token> output)
        {
            TryInvalid(context, out FunctionOverloadList? overloads);
            if (AppliedFunction is not null) output.Add(new(TokenProperty.Function, AppliedFunction, new FunctionToken(context.GetFunction(overloads!, Parameters))));
        }

        /// <summary>
        /// Pushes a function into the output stack directly.
        /// </summary>
        /// <param name="context">The CalculatorContext.</param>
        /// <param name="appliedFunction">The function to apply.</param>
        /// <param name="parameters">The number of parameters to account for.</param>
        /// <param name="output">The output stack.</param>
        public static void PushFunctionInto(CalculatorContext context, string appliedFunction, int parameters, List<Token> output)
        {
            FunctionOverloadList overloads = context.GetFunctionFromName(appliedFunction, out _)!;
            output.Add(new(TokenProperty.Function, appliedFunction, new FunctionToken(context.GetFunction(overloads, parameters))));
        }
    }

    file record StateFields
    {
        public static State Fields => new(new() {
            { "Expected", ExpectedForm.Operand },
            { "BracketStack", new Stack<BracketEntry>() },
            { "PotentialFunction", null },
            { "IsTransitive", false }
        });
    }

    public class ArithmeticParser : IParser
    {
        public readonly List<Token> Output = [];
        List<Token> IParser.Output => Output;

        private readonly Stack<Operator> shuntingStack = [];
        private CalculatorContext calculatorContext = default!;

        private State state;

        private void PopAndPushOperators(Operator operatorObject, Operator? ending = null)
        {
            bool isRightToLeft = operatorObject.HasProperty(OperatorProperty.RightToLeft);
            while (true)
            {
                if (shuntingStack.Count == 0) break;
                Operator first = shuntingStack.First();
                if (isRightToLeft && operatorObject == first || operatorObject > first) break;
                if (first == ending) return;
                Output.Add(new(TokenProperty.Operator, shuntingStack.Pop().Symbol));
            }

            if (ending is Operator endOperator && (shuntingStack.Count == 0 || shuntingStack.First() != endOperator)) throw new OperatorFormatException($"Expected {endOperator}, got {shuntingStack.First()}");
        }

        private void CloseTransitivity(bool isTransitive)
        {
            if (isTransitive)
            {
                Output.Add(new(TokenProperty.Operand, "", new VoidToken()));
                state.Set("IsTransitive", false);
            }
        }

        private void PushOperator(Token token)
        {
            ExpectedForm newExpected = ExpectedForm.Operand;
            ExpectedForm expected = state.Get<ExpectedForm>("Expected");
            Stack<BracketEntry> bracketStack = state.Get<Stack<BracketEntry>>("BracketStack");
            bool isTransitive = state.Get<bool>("IsTransitive");

            Operator operatorObject = calculatorContext.DetermineOperationFromString(token.Source);
            if (operatorObject.HasProperty(OperatorProperty.ClosedBracket))
            {
                if (expected.HasFlag(ExpectedForm.Operand)) throw new OperatorFormatException($"Expected operand, got {operatorObject}");
                PopAndPushOperators(operatorObject, operatorObject.Opposite!);
                shuntingStack.Pop();
                bracketStack.Pop().TryPushInto(calculatorContext, Output);
                CloseTransitivity(isTransitive);
                state.Set("Expected", ExpectedForm.Operator | ExpectedForm.Implicit);
                return;
            }
            if (operatorObject.HasProperty(OperatorProperty.Bracket))
            {
                if (expected.HasFlag(ExpectedForm.Implicit)) PushOperator(new(TokenProperty.Operator, "*"));
                else if (!expected.HasFlag(ExpectedForm.Operand)) throw new OperatorFormatException($"Expected operand, got {operatorObject}");
                bracketStack.Push(new(operatorObject.Opposite!, state.Get<string?>("PotentialFunction", null)));
                shuntingStack.Push(operatorObject);
                state.Set("Expected", ExpectedForm.Operand | ExpectedForm.Parameter);
                state.Set<string?>("PotentialFunction", null);
                return;
            }
            if (operatorObject.HasProperty(OperatorProperty.Separator))
            {
                if (expected.HasFlag(ExpectedForm.Operand)) throw new OperatorFormatException($"Expected operand, got {operatorObject}");
                PopAndPushOperators(operatorObject, bracketStack.First().ClosingBracket!.Opposite!);
                CloseTransitivity(isTransitive);
                state.Set("Expected", ExpectedForm.Operand | ExpectedForm.Parameter);
                return;
            }

            if (operatorObject.HasProperty(OperatorProperty.UnaryPotential))
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
                PopAndPushOperators(operatorObject);
            }

            if (operatorObject.HasProperty(OperatorProperty.Transitive)) state.Set("IsTransitive", true);

            if (!operatorObject.HasProperty(OperatorProperty.Ignore)) shuntingStack.Push(operatorObject);
            state.Set("Expected", newExpected);
        }

        private void PushOperand(Token token)
        {
            ExpectedForm expected = state.Get<ExpectedForm>("Expected");
            if (!expected.HasFlag(ExpectedForm.Operand) && !expected.HasFlag(ExpectedForm.Implicit)) throw new InvalidTokenException($"Expected operand, got {token}");
            if (expected.HasFlag(ExpectedForm.Implicit))
            {
                //if (token.Type == TokenType.Number) throw new InvalidTokenException($"Implicit multiplication cannot be applied to {token}");
                // Only implement if implicit numbering is strictly prohibited by other arithmetic interactions.
                PushOperator(new(TokenProperty.Operator, "*"));
            }
            if (token.HasProperty(TokenProperty.Function))
            {
                if (state.Get<string?>("PotentialFunction", null) is not null) throw new InvalidTokenException($"Cannot have function {token} in implicitly-invoked function");
                state.Set("PotentialFunction", token.Source);
                state.Set("Expected", ExpectedForm.Operand);
            }
            else
            {
                Output.Add(token);
                state.Set("Expected", ExpectedForm.Operator | ExpectedForm.Implicit);

                if (state.Get<string?>("PotentialFunction", null) is string potentialFunction)
                {
                    state.Set<string?>("PotentialFunction", null);
                    BracketEntry.PushFunctionInto(calculatorContext, potentialFunction, 1, Output);
                }
            }
        }

        public void Parse(in Token[] tokens, CalculatorContext context)
        {
            Output.Clear();
            shuntingStack.Clear();

            state = StateFields.Fields;
            state.Set("Context", context);

            calculatorContext = context;
            Stack<BracketEntry> bracketStack = state.Get<Stack<BracketEntry>>("BracketStack");

            ExpectedForm expected;
            foreach (Token token in tokens)
            {
                expected = state.Get<ExpectedForm>("Expected");
                if (expected.HasFlag(ExpectedForm.Parameter))
                {
                    BracketEntry entry = bracketStack.First();
                    entry.Parameters++;
                    state.Set("Expected", expected & ~ExpectedForm.Parameter);
                }
                if (token.HasProperty(TokenProperty.Operator)) PushOperator(token);
                if (token.HasProperty(TokenProperty.Operand)) PushOperand(token);
            }

            expected = state.Get<ExpectedForm>("Expected");
            if (expected.HasFlag(ExpectedForm.Operand)) throw new OperatorFormatException($"Expected operand at the end");

            while (shuntingStack.Count > 0)
            {
                Operator last = shuntingStack.Pop();
                if (last.HasProperty(OperatorProperty.Bracket)) throw new OperatorFormatException("Mismatched brackets");
                Output.Add(new(TokenProperty.Operator, last.Symbol));
            }
        }
    }
}
