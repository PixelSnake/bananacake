namespace BCake.Parser.Exceptions {
    public class UndefinedSymbolException : Error {
        public UndefinedSymbolException(
            BCake.Parser.Token token,
            string name,
            BCake.Parser.Syntax.Scopes.Scope scope
        )
            : base($"Undefined symbol - the symbol \"{name}\" has not been declared in this scope\n\tat scope {scope.FullName}", token)
        {}
    }
}