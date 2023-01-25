using BCake.Parser;
using BCake.Runtime;
using BCake.Parser.Syntax.Types;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Runtime.Nodes.Value;

namespace BCake.Std {
    public class BoolToStringCast : NativeFunctionType {
        public static readonly NativeFunctionType Implementation = new BoolToStringCast();

        public override bool ExpectsThisArg => true;

        private BoolToStringCast() : base(
            BoolValueNode.Type.Scope,
            StringValueNode.Type,
            "!as_string",
            new ParameterType[] {

            }
        ) {}

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
                        
            return new RuntimeStringValueNode(
                new StringValueNode(
                    DefiningToken,
                    ((bool)arguments[0].Value).ToString().ToLower()
                ),
                null
            );
        }
    }
}
