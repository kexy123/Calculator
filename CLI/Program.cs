using CLI.Elements.Input;
using CLI.Elements.Text;
using CLI.Pages;
using Core.AssetContexts;
using Core.Expression;
using Core.Value;

namespace CLI
{
    file record MenuOptions
    {
        public static Dictionary<char, string> Options = new()
        {
            { 'C', "Calculate" },
            { 'V', "Variable editing" },
            { 'F', "Function editing" },
            { 'L', "Clear canvas" },
            { 'X', "Terminate" },
        };

        public static char DefaultOption = 'C';

        //public static 
    }

    internal class Program
    {
        static void Initialize()
        {
            StyleList.AddStyles([
                new Style("Default"),
                new Style("DefaultHighlighted", ConsoleColor.Black, ConsoleColor.White),

                new Style("Header", ConsoleColor.Yellow),

                new Style("InputLabel", ConsoleColor.Magenta),

                new Style("Error", ConsoleColor.Red),
                new Style("Process", ConsoleColor.DarkYellow),
                new Style("Success", ConsoleColor.Green),
            ]);
        }

        static void Main(string[] args)
        {
            Initialize();

            CalculatorContext context = new(new Arithmetic());
            context.AssignVariable("pi", new NumberToken(Math.PI));
            context.AssignVariable("e", new NumberToken(Math.E));

            context.AssignVariable("TRUE", new BooleanToken(true));
            context.AssignVariable("FALSE", new BooleanToken(false));

            context.AssignFunction(new("sqrt", "n^0.5", ["n"]));

            while (true)
            {
                Text.WriteLine("=====CALCULATOR (@kexy321)=====", "Header");
                string option = MenuSelection.ReadOption(MenuOptions.Options, MenuOptions.DefaultOption);

                Text.EmptySpace();

                switch (option)
                {
                    case "Calculate":
                        Evaluation.Evaluate(context);
                        Text.EmptySpace();
                        break;
                    case "Terminate":
                        return;
                    case "Clear canvas":
                        Console.Clear();
                        break;
                    default:
                        throw new NotImplementedException("Did not implement " + option);
                }
            }
        }
    }
}
