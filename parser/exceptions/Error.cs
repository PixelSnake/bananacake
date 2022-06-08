using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Exceptions {
    public class Error : ExceptionBase {
        public Error(string message, BCake.Parser.Token token)
            : base($"Error in {token.FilePath}({token.Line},{token.Column}): {message}")
        {}
    }
}