using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Errors {
    public class InvalidCastError : Error {
        public InvalidCastError(Type left, Type right, Token token)
            : base($"Cannot cast {left.FullName} to {right.FullName} because there is no matching cast method", token)
        {}
    }
}