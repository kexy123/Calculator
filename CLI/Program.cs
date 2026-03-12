using Core.AssetContexts;
using Core.Expression;

namespace CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculatorContext context = new(new Arithmetic());
            context.TokenizeExpression("2 + 2 * 2 - 2 / 2");
            Console.WriteLine(context.Tokenizer);
        }
    }
}
