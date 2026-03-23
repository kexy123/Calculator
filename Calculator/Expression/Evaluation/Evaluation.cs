using Core.Operation;
using Core.Value;
using Core.Variable;

namespace Core.Expression.Evaluation
{
    using ValueStack = Stack<IValue>;

    public class Evaluator(CalculatorContext context)
    {
        public ValueStack OutputStack = [];
        public CalculatorContext Context = context;

        private BooleanToken? transitiveValue;

        private void PushTransitivity()
        {
            if (transitiveValue is not null)
            {
                OutputStack.Pop();
                OutputStack.Push(transitiveValue);
                transitiveValue = null;
            }
        }

        private void PushOperand(Token.Token operand)
        {
            PushTransitivity();

            switch (operand.Type)
            {
                case Token.TokenProperty.Number:
                    if (operand.Value is not VoidToken) OutputStack.Push(operand.Value!);
                    break;
                case Token.TokenProperty.Variable:
                    OutputStack.Push(operand.Value!);
                    break;
            }
        }

        private void PerformOperator(Token.Token token)
        {
            Operator operatorObject = Context.DetermineOperationFromString(token.Source);
            IValue right = OutputStack.Pop();
            IValue result;

            if (!operatorObject.HasProperty(OperatorProperty.Transitive)) PushTransitivity();

            IValue left = OutputStack.Pop();

            if (operatorObject.HasProperty(OperatorProperty.Unary)) result = operatorObject.Execute(new NothingToken(), right, Context);
            else result = operatorObject.Execute(left, right, Context);

            if (operatorObject.HasProperty(OperatorProperty.Transitive))
            {
                if (transitiveValue is not null) transitiveValue &= (BooleanToken)result;
                else transitiveValue = (BooleanToken)result;
                OutputStack.Push(right);
            }
            else OutputStack.Push(result);
        }

        private void PerformFunction(Token.Token token)
        {
            PushTransitivity();

            if (token.Value is FunctionToken functionObject)
            {
                Function func = functionObject.Value;
                List<IValue> arguments = [];
                for (int i = 0; i < func.ParameterCount; i++) arguments.Add(OutputStack.Pop());
                arguments.Reverse();
                IValue result = func.Invoke(Context, [.. arguments]);
                OutputStack.Push(result);
            }
        }

        /// <summary>
        /// Evaluates the given list of tokens.
        /// </summary>
        /// <param name="tokens">The tokens to evaluate.</param>
        /// <param name="context">The calculator context.</param>
        /// <returns>The final value.</returns>
        /// <exception cref="InvalidTokenException">Thrown in any invalid token form.</exception>
        public IValue Evaluate(Token.Token[] tokens)
        {
            OutputStack.Clear();

            foreach (Token.Token token in tokens)
            {
                if (token.HasProperty(Token.TokenProperty.Function)) PerformFunction(token);
                else if (token.HasProperty(Token.TokenProperty.Operand)) PushOperand(token);
                else if (token.HasProperty(Token.TokenProperty.Operator)) PerformOperator(token);
                else throw new InvalidTokenException($"Invalid token {token}");
            }

            PushTransitivity();
            return OutputStack.First();
        }
    }
}
