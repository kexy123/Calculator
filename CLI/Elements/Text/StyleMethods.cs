namespace CLI.Elements.Text
{
    public static class StyleMethods
    {
        /// <summary>
        /// Flips the console colors.
        /// </summary>
        public static void FlipConsoleColors()
        {
            (Console.ForegroundColor, Console.BackgroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
        }

        /// <summary>
        /// Uses default console colors.
        /// </summary>
        public static void DefaultConsoleColors()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
