using BCake.Parser;
using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;

namespace BCake.Array {
    public class Array : GenericClassType {
        public static Array Implementation = new();

        private Array() : base(
            Namespace.Global.Scope,
            Access.@public,
            "Array",
            Token.NativeCode(),
            new Token[]
            {
                Token.Anonymous("private"),
                Token.Anonymous("int"),
                Token.Anonymous("__id"),
                Token.Anonymous(";"),
            },
            new Token[]
            {
                Token.Anonymous("T"),
                Token.Anonymous(">"),
            }
        ) {}

        public override void ParseInner() {

            base.ParseInner();

            Scope.Declare(ArrayConstructor.Implementation);
            Scope.Declare(OperatorIndex.Implementation);
        }
    }
}