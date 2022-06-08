using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Errors {
    public class InvalidReturnError : Error {
        public InvalidReturnError(BCake.Parser.Token token)
            : base($"Invalid return statement - there is nothing to return from", token)
        {}
    }
}