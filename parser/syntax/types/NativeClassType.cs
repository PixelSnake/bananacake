namespace BCake.Parser.Syntax.Types {
    public abstract class NativeClassType : ClassType {
        public NativeClassType(Scopes.Scope scope, Access access, string name) : base(scope, access, name, null, null) {}

        public override void ParseInner() {}
    }
}