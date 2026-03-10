using Calculator.Operation;
using Calculator.Value;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Number a = Convert.ToDouble(Console.ReadLine()), b = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine(ArithmeticOperators.Exponentiation.Execute(a, b));
        }
    }
}
