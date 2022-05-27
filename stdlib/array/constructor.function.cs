using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Array {
    public class ArrayConstructor : NativeFunctionType {
        public static NativeFunctionType Implementation = new ArrayConstructor();

        private ArrayConstructor() : base(
            Array.Implementation.Scope,
            Array.Implementation,
            "!constructor",
            new ParameterType[] {},
            new ArrayConstructor[] {}
        ) { }

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
            return new RuntimeNullValueNode(DefiningToken);
        }
    }
}