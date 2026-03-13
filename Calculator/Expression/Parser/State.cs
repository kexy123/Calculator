using System.Diagnostics.CodeAnalysis;

namespace Core.Expression.Parser
{
    using StateDictionary = Dictionary<string, object>;

    public readonly struct State(StateDictionary? states = null)
    {
        private readonly StateDictionary States = states ?? [];

        /// <summary>
        /// Sets a state.
        /// </summary>
        /// <param name="name">The name of the state.</param>
        /// <param name="value">The value of the state.</param>
        public readonly void Set(string name, object value) => States.Add(name, value);

        /// <summary>
        /// Gets a state.
        /// </summary>
        /// <param name="name">The name of the value of the state.</param>
        /// <returns>The value of the state.</returns>
        public readonly object Get(string name) => States[name];

        public static implicit operator State(StateDictionary states) => new(states);
    }
}
