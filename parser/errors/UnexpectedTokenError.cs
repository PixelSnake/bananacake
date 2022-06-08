namespace BCake.Parser.Errors {
    public class UnexpectedTokenError : Error {
        public UnexpectedTokenError(Token token)
            : base($"Unexpected token \"{token.Value}\"", token)
        {}
    }
}