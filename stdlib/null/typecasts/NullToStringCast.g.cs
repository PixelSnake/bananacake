using BCake.Parser;
using BCake.Runtime;
using BCake.Parser.Syntax.Types;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Runtime.Nodes.Value;

namespace BCake.Std {
    public class NullToStringCast : NativeFunctionType {
        public static readonly NativeFunctionType Implementation = new NullToStringCast();

        public override bool ExpectsThisArg => true;

        private NullToStringCast() : base(
            NullValueNode.Type.Scope,
            StringValueNode.Type,
            "!as_string",
            new ParameterType[] {

            }
        ) {}

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
                        
            return new RuntimeStringValueNode(new StringValueNode(DefiningToken, "null"), null);
        }
    }
}
