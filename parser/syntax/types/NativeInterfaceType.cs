namespace BCake.Parser.Syntax.Types
{
    public abstract class NativeInterfaceType : InterfaceType
    {
        public NativeInterfaceType(Scopes.Scope scope, string name) : base(Token.NativeCode(), scope, name, null) { }

        public override void ParseInner() { }
    }
}
