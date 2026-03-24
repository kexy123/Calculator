using System.Diagnostics.CodeAnalysis;

namespace CLI.Elements.Input
{
    using OptionList = Dictionary<char, string>;

    [method: SetsRequiredMembers]
    public class MenuSelection(OptionList options, char defaultOption)
    {
        public required OptionList Options = options;
        public required char DefaultOption = defaultOption;
        public required char CurrentOption = defaultOption;

        public readonly KeyValuePair<char, string>[] OptionArray = [.. options];

        private int OptionIndex => GetIndexFromCharacterOption();
        private bool hasLogged = false;

        public void LogOptions()
        {
            if (hasLogged) Console.SetCursorPosition(0, Console.CursorTop - Options.Count);
            hasLogged = true;
            foreach (KeyValuePair<char, string> option in Options)
            {
                string optionText = $"[{option.Key}]  {option.Value}";
                Text.Text.WriteLine(optionText + new string(' ', Console.WindowWidth - optionText.Length), option.Key == CurrentOption ? "DefaultHighlighted" : "Default");
            }
        }

        public int GetIndexFromCharacterOption()
        {
            int index = 0;
            foreach (KeyValuePair<char, string> option in Options)
            {
                if (option.Key == CurrentOption) return index;
                ++index;
            }
            throw new InvalidDataException($"Current option '{CurrentOption}' is not a valid option.");
        }

        public void OffsetOption(int indexOffset)
        {
            int current = (OptionIndex + indexOffset);
            if (current < 0) current = OptionArray.Length - 1;
            else if (current >= OptionArray.Length) current = 0;
            CurrentOption = OptionArray[current].Key;
        }

        public void ReadUserInput()
        {
            LogOptions();
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                char key = char.ToUpper(keyInfo.KeyChar);
                if (Options.ContainsKey(key))
                {
                    CurrentOption = key;
                    LogOptions();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Enter) break;
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    CurrentOption = DefaultOption;
                    LogOptions();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    OffsetOption(-1);
                    LogOptions();
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    OffsetOption(1);
                    LogOptions();
                }
                else Console.Beep();
            }
        }

        public static string ReadOption(OptionList options, char defaultOption)
        {
            Console.CursorVisible = false;
            MenuSelection selection = new(options, defaultOption);
            selection.ReadUserInput();
            Console.CursorVisible = true;
            return options[selection.CurrentOption];
        }
    }
}
