using CLI.Display;
using Core.AssetContexts;
using Core.Expression;

namespace CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculatorContext context = new(new Arithmetic());
            string? value = Console.ReadLine();
            context.TokenizeExpression(value!);
            Console.WriteLine(List.ToString(context.Tokenizer.Tokens));
            context.Parse(context.Tokenizer.Tokens);
            Console.WriteLine(List.ToString(context.Parser.Output.ToArray()));
        }
    }
}
