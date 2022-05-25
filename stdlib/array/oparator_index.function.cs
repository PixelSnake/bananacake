using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Array {
    public class OperatorIndex : NativeFunctionType {
        public static NativeFunctionType Implementation = new OperatorIndex();
        public override bool ExpectsThisArg => true;

        private OperatorIndex() : base(
            Array.Implementation.Scope,
            IntValueNode.Type, // TODO: generic
            "!operator_index",
            new ParameterType[] {
                 new ParameterType(null, IntValueNode.Type, "i")
            },
            new OperatorIndex[] {}
        ) { }

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
            var index = (int)arguments[1].Value;

            return new RuntimeIntValueNode(
                new IntValueNode(
                    DefiningToken,
                    1337 * index
                ),
                null
            );
        }
    }
}