using Type = BCake.Parser.Syntax.Types.Type;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators
{
    public interface ISymbol
    {
        public Type Symbol { get; }
    }
}
