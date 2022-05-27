using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Std.IStringCast
{
    public class CastToString : NativeFunctionType
    {
        public static CastToString Implementation = new CastToString();
      
        public override bool ExpectsThisArg => true;

        public CastToString() : base(
            IStringCast.Implementation.Scope,
            StringValueNode.Type,
            "!as_string",
            new ParameterType[] { }
        ) { }

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments)
        {
            return new RuntimeStringValueNode(
                new StringValueNode(DefiningToken, arguments[0].ToString()),
                null
            );
        }
    }
}
