using Core.Common;
using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Operation;
using Core.Variable;
using System.Diagnostics.CodeAnalysis;

namespace Core.AssetParsers
{
    file enum ExpectedForm
    {
        Operand,
        OperatorOrImplicit,
        Operator
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
        public readonly void ThrowOnInvalid()
        {
            if (AppliedFunction == null && Parameters != 1) throw new MathArgumentException("Expected 1 parameter in bracket, got " + Parameters);

            uint count = AppliedFunction!.Value.ParameterCount;
            if (Parameters != count) throw new MathArgumentException($"Expected {count} parameter(s) in function, got " + Parameters);
        }
    }

    file record StateFields
    {
        public static State Fields = new(new() {
            { "Expected", ExpectedForm.Operand },
            { "BracketStack", new Stack<BracketEntry>() },
            { "ShuntingStack", new Stack<Token>() },
        });
    }

    public class Arithmetic : IParser
    {
        public readonly List<Token> Output = [];
        List<Token> IParser.Output => Output;

        public void Parse(in Token[] tokens)
        {
            // Dijkstra's extension of the Shunting Yard algorithm.
            Output.Clear();
            State state = StateFields.Fields;

            foreach (Token token in tokens)
            {
                switch (token.Type)
                {
                    case (TokenType.Operator):
                        //if (state.Get("Expected"))
                        break;
                }
            }
            // TODO: Create Arithmetic parser here.
        }
    }
}
