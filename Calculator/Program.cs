using Calculator.Contexts;
using Calculator.Expression.Token;
using Calculator.Value;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Number a = Convert.ToDouble(Console.ReadLine()), b = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine(Operators.Exponentiation.Execute(a, b));

            Tokenizer tokenize = new("5.6 28", new(), TokenPatterns.ExpressionPatterns);
            tokenize.Tokenize();
            Console.WriteLine(tokenize);
        }
    }
}
