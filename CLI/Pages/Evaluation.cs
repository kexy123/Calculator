using CLI.Elements;
using CLI.Elements.Text;
using Core.Expression;
using Core.Expression.Parser;
using Core.Expression.Token;
using Core.Value;

namespace CLI.Pages
{
    public static class Evaluation
    {
        public static void Evaluate(CalculatorContext context)
        {
            Text.WriteLine("=====CALCULATE EXPRESSION=====", "Header");
            Text.Write("Enter an expression to evaluate: ", "InputLabel");
            string? value = Console.ReadLine();
            if (value is null || value == "") return;

            IValue? result = null;
            try
            {
                result = context.Evaluate(value);
                Text.EmptySpace();
            }
            catch (Exception ex)
            {
                Text.WriteLine(ex.Message, "Error");
            }
            finally
            {
                if (result is not null)
                {
                    Tokenizer tokens = context.TokenizeExpression(value);
                    Text.WriteLine($"Your expression was tokenized:\n    {List.ToString(tokens.Tokens)}\n", "Process");
                    IParser parser = context.Parse(tokens);
                    Text.WriteLine($"Your expression was parsed:\n    {List.ToString(parser.Output)}\n", "Process");
                    Text.WriteLine($"ANS =\n    {result}", "Success");
                    context.AssignVariable("ANS", result);
                }
            }
        }
    }
    }
