namespace BCake.Parser.Exceptions {
    public class UnexpectedTypeParameterException : Error {
        public UnexpectedTypeParameterException(BCake.Parser.Token token)
            : base($"Unexpected generic type parameter list - only classes can be generic", token)
        {}
    }
}