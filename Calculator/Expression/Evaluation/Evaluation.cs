using Core.Operation;
using Core.Value;

namespace Core.Expression.Evaluation
{
    using ValueStack = Stack<IValue>;

    public static class Evaluator
    {
        private static void PushOperand(Token.Token operand, ValueStack outputStack)
        {
            switch (operand.Type)
            {
                case Token.TokenType.Number:
                    outputStack.Push(operand.Value!);
                    break;
            }
        }

        private static void PerformOperator(Token.Token token, ValueStack outputStack, CalculatorContext context)
        {
            Operator operatorObject = context.DetermineOperationFromString(token.Source);
            IValue first = outputStack.Pop();
            IValue result;
            if (operatorObject.ContainsProperty(OperatorProperty.Unary)) result = operatorObject.Execute(new Nothing(), first);
            else result = operatorObject.Execute(outputStack.Pop(), first);
            outputStack.Push(result);
        }

        /// <summary>
        /// Evaluates the given list of tokens.
        /// </summary>
        /// <param name="tokens">The tokens to evaluate.</param>
        /// <param name="context">The calculator context.</param>
        /// <returns>The final value.</returns>
        /// <exception cref="InvalidTokenException">Thrown in any invalid token form.</exception>
        public static IValue Evaluate(Token.Token[] tokens, CalculatorContext context)
        {
            ValueStack outputStack = [];

            foreach (Token.Token token in tokens)
            {
                if (token.ContainsProperty(Token.TokenType.Operand)) PushOperand(token, outputStack);
                else if (token.ContainsProperty(Token.TokenType.Operator)) PerformOperator(token, outputStack, context);
                else throw new InvalidTokenException($"Invalid token {token}");
            }

            return outputStack.First();
        }
    }
}
