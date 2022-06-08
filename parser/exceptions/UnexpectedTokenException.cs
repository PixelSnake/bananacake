namespace BCake.Parser.Exceptions {
    public class UnexpectedTokenException : Error {
        public UnexpectedTokenException(BCake.Parser.Token token)
            : base($"Unexpected token \"{token.Value}\"", token)
        {}
    }
}