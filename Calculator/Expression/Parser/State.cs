using System.Diagnostics.CodeAnalysis;

namespace Core.Expression.Parser
{
    using StateDictionary = Dictionary<string, object?>;

    public readonly struct State(StateDictionary? states = null)
    {
        private readonly StateDictionary States = states ?? [];

        /// <summary>
        /// Sets a state.
        /// </summary>
        /// <param name="name">The name of the state.</param>
        /// <param name="value">The value of the state.</param>
        public readonly void Set<T>(string name, T value)
        {
            if (!States.TryAdd(name, value)) States[name] = value;
        }

        /// <summary>
        /// Gets a state.
        /// </summary>
        /// <typeparam name="T">The expected type of the value to retrieve.</typeparam>
        /// <param name="name">The name of the value of the state.</param>
        /// <returns>The value of the state.</returns>
        /// <exception cref="InvalidDataException">Thrown when the state object is not of the specified type.</exception>
        public readonly T Get<T>(string name)
        {
            if (States.TryGetValue(name, out object? value))
            {
                if (value is T specified) return specified;
            }
            throw new InvalidDataException($"State object '{name}' is not {typeof(T).Name}");
        }
        /// <summary>
        /// Retrieves the value associated with the specified name, or returns the provided fallback value if the name is not found or the value is not of the expected type.
        /// </summary>
        /// <typeparam name="T">The expected type of the value to retrieve.</typeparam>
        /// <param name="name">The key of the value to retrieve.</param>
        /// <param name="fallback">The value to return if the specified name does not exist or is not of the type.</param>
        /// <returns>The value associated with the specified name if it exists and is of the type. Otherwise, the fallback value.</returns>
        public readonly T Get<T>(string name, T fallback)
        {
            if (States.TryGetValue(name, out object? value)) if (value is T specified) return specified;
            return fallback;
        }

        public static implicit operator State(StateDictionary states) => new(states);
    }
}
