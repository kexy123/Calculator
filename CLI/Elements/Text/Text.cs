namespace CLI.Elements.Text
{
    public static class Text
    {
        /// <summary>
        /// Writes a line in a given style. The style is applied before writing the line and reset to default after writing.
        /// </summary>
        /// <param name="text">The text to put in as a single line.</param>
        /// <param name="styleName">The Style to apply.</param>
        public static void WriteLine(string text, string styleName = "Default")
        {
            StyleList.GetStyle(styleName).Apply();
            Console.WriteLine(text);
            StyleList.GetStyle("Default").Apply();
        }

        /// <summary>
        /// Write text in a given style. The style is applied before writing the text and reset to default after writing.
        /// </summary>
        /// <param name="text">The text to put in.</param>
        /// <param name="styleName">The Style to apply.</param>
        public static void Write(string text, string styleName = "Default")
        {
            StyleList.GetStyle(styleName).Apply();
            Console.Write(text);
            StyleList.GetStyle("Default").Apply();
        }

        /// <summary>
        /// Creates a specified amount of empty lines. This is used to create space between elements in the console.
        /// </summary>
        /// <param name="amount">The amount of empty lines. Defaults to 1.</param>
        public static void EmptySpace(int amount = 1)
        {
            Console.WriteLine(new string('\n', amount - 1));
        }
    }
}
