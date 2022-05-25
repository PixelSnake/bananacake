using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Std {
    public class Print : NativeFunctionType {
        public static NativeFunctionType Implementation = new Print();
        public override bool ExpectsThisArg => false;

        private Print() : base(
            Namespace.Global.Scope,
            null,
            "print",
            new ParameterType[] {
                 new ParameterType(null, StringValueNode.Type, "s")
            }
        ) {}

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
            System.Console.Write(arguments[0].Value);

            return new RuntimeNullValueNode(DefiningToken);
        }
    }
}