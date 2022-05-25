
namespace BCake.Parser.Syntax.Types {
    public class PlaceholderType : Types.Type {
        public PlaceholderType(Scopes.Scope scope, string name, Token definingToken)
            : base(scope, name, null)
        {
            DefiningToken = definingToken;
        }
    }
}