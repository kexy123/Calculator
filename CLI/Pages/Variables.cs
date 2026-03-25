using CLI.Elements.Text;
using Core.Expression;
using Core.Value;

namespace CLI.Pages
{
    public static class Variables
    {
        public static void ShowVariables(CalculatorContext context)
        {
            Text.WriteLine("=====VARIABLE LIST=====", "Header");
            Text.Write("Use the '->' or '=' operation in the calculator to assign a variable to a value.\n", "Default");
            foreach (KeyValuePair<string, IValue> entry in context.Variables)
            {
                Text.WriteLine($"\t{entry.Key} = {entry.Value}", "Default");
            }
            Text.EmptySpace();
        }
    }
}
