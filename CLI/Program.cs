using CLI.Display;
using Core.AssetContexts;
using Core.Expression;
using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Value;

namespace CLI
{
    file record A
    {
        public static IValue Prod(IValue[] values)
        {
            NumberToken sum = new(1);
            foreach (IValue value in values)
            {
                sum *= (NumberToken)value;
            }
            return sum;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            CalculatorContext context = new(new Arithmetic());
            context.AssignVariable("pi", new NumberToken(Math.PI));
            context.AssignVariable("e", new NumberToken(Math.E));

            context.AssignFunction(new("prod", A.Prod, -1));
            context.AssignFunction(new("prod", A.Prod, -1));

            while (true)
            {
                Console.Write("Enter an expression to evaluate: ");
                string? value = Console.ReadLine();
                if (value is null || value == "") return;

                //Tokenizer tokens = context.TokenizeExpression(value);
                //Console.WriteLine(List.ToString(tokens.Tokens));
                //IParser parser = context.Parse(tokens);
                //Console.WriteLine(List.ToString(parser.Output));
                //IValue result = context.EvaluateTokens(parser);
                //Console.WriteLine(result);

                try
                {
                    IValue result = context.Evaluate(value);
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
