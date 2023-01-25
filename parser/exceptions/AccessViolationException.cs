namespace BCake.Parser.Exceptions {
    public class AccessViolationException : Error {
        public AccessViolationException(
            Token token,
            Syntax.Types.Type member,
            Syntax.Scopes.Scope sourceScope
        )
            : base($"Cannot access {member.Access} symbol \"{member.FullName}\" from current scope due to it's protection level", token)
        {}
    }
}