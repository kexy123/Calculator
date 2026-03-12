using Calculator.Contexts;
using Calculator.Expression;
using Calculator.Expression.Token;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculatorContext context = new(new Arithmetic());

            Tokenizer tokenize = new("5.6 + 28", context);
            tokenize.Tokenize();
            Console.WriteLine(tokenize);
        }
    }
}
