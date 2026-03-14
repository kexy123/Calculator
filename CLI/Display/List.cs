namespace CLI.Display
{
    public static class List
    {
        public static string ToString<T>(IEnumerable<T> list) => "[" + string.Join(", ", list) + "]";
    }
}