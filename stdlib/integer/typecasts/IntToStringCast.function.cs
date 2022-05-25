using BCake.Runtime;
using BCake.Parser.Syntax.Types;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Runtime.Nodes.Value;

namespace BCake.Integer.Typecasts {
    /// <summary>
    /// Native function to convert integer to string.
    ///
    /// Usage: &lt;int&gt; as string
    /// </summary>
    public class IntToStringCast : NativeFunctionType
    {
        public static NativeFunctionType Implementation = new IntToStringCast();
        public override bool ExpectsThisArg => true;

        private IntToStringCast() : base(
            IntValueNode.Type.Scope,
            null,
            "!as_string",
            new ParameterType[] {}
        ) {}

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
            return new RuntimeStringValueNode(
                new StringValueNode(
                    DefiningToken,
                    ((int)arguments[0].Value).ToString()
                ),
                null
            );
        }
    }
}