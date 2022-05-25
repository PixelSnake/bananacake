namespace BCake.Parser.Syntax.Types {
    public abstract class ComplexType : Type {
        public Token[] Tokens { get; protected set; }

        public ComplexType(Scopes.Scope scope, string name)
            : base(scope, name, null) {}
        public ComplexType(Scopes.Scope scope, string name, Access access)
            : base(scope, access, name, null) {}

        public abstract void ParseInner();
    }
}