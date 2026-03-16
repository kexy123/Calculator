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
            string? value = Console.ReadLine();
            IValue result = context.Evaluate(value!);
            Console.WriteLine(result);
        }
    }
}
