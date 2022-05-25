using Type = BCake.Parser.Syntax.Types.Type;

namespace BCake.Parser.Exceptions
{
    public class MissingTypeArgumentsException : TokenException
    {
        public MissingTypeArgumentsException(Token token, Type symbol) : base(
            $"Illegal use of generic symbol - the symbol {symbol.FullName} was used without providing type arguments",
            token) {}
    }
}
