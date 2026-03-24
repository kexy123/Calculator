using System.Diagnostics.CodeAnalysis;

namespace CLI.Elements.Text
{
    public static class StyleList
    {
        private readonly static Dictionary<string, Style> styles = [];

        /// <summary>
        /// Adds a style to the style list.
        /// </summary>
        /// <param name="style">The style to add.</param>
        public static void AddStyle(Style style)
        {
            if (!styles.TryAdd(style.Name, style)) styles[style.Name] = style;
        }

        /// <summary>
        /// Adds each style to the style list.
        /// </summary>
        /// <param name="styleList">The list of styles to add.</param>
        public static void AddStyles(Style[] styleList)
        {
            foreach (Style style in styleList) AddStyle(style);
        }

        /// <summary>
        /// Gets a Style from the style list.
        /// </summary>
        /// <param name="name">The name of the style.</param>
        /// <returns>The Style.</returns>
        public static Style GetStyle(string name) => styles[name];
    }

    [method: SetsRequiredMembers]
    public class Style(string name, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
    {
        public required string Name = name;
        public required ConsoleColor Foreground = foreground;
        public required ConsoleColor Background = background;

        /// <summary>
        /// Applies the given style.
        /// </summary>
        public void Apply()
        {
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
        }
    }
}
