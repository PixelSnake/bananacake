namespace BCake.Runtime.Exceptions {
    public class RuntimeException : System.Exception {
        public RuntimeException(string message, BCake.Parser.Token token)
            : base($"{token.FilePath}({token.Line},{token.Column}): {message}")
        {}
    }
}