using CLI.Display;
using Core.AssetContexts;
using Core.Expression;
using Core.Value;

namespace CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculatorContext context = new(new Arithmetic());
            context.AssignVariable("pi", new Number(Math.PI));
            context.AssignVariable("e", new Number(Math.E));

            context.AssignFunction(new("foo", "a+b", ["a", "b"]));

            while (true)
            {
                Console.Write("Enter an expression to evaluate: ");
                string? value = Console.ReadLine();
                if (value is null || value == "") return;

                //Token[] tokens = context.TokenizeExpression(value);
                //Console.WriteLine(List.ToString(tokens));
                //tokens = context.Parse(tokens);
                //Console.WriteLine(List.ToString(tokens));
                //IValue result = context.EvaluateTokens(tokens);

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
