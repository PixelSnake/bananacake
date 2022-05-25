using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Types;

namespace BCake.Array {
    public class Array : NativeClassType {
        public static Array Implementation = new Array();

        private Array() : base(
            Namespace.Global.Scope,
            Access.@public,
            "Array"
        ) {}

        public override void ParseInner() {
            Scope.Declare(ArrayConstructor.Implementation);
            Scope.Declare(OperatorIndex.Implementation);
        }
    }
}