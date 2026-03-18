using CLI.Display;
using Core.AssetContexts;
using Core.Expression;
using Core.Expression.Token;
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

            string? value = Console.ReadLine();
            IValue result = context.Evaluate(value!);
            Console.WriteLine(result);
        }
    }
}
