using Calculator.Value;
using Calculator.Variable;

namespace Calculator.Expression
{
    public struct CalculatorContext : IVariableContext
    {
        public Dictionary<string, Function> Functions => Functions;
        public Dictionary<string, IValue> Variables => Variables;

        public void ImplementFunction(Function function) => Functions.Add(function.Name, function);
        public void ImplementVariable(string name, IValue value) => Variables.Add(name, value);
    }
}
