using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Std {
    public class Println : NativeFunctionType {
        public static NativeFunctionType Implementation = new Println(IStringCast.IStringCast.Implementation, true);
        public override bool ExpectsThisArg => false;

        private Println(Type paramType, bool initOverloads = false) : base(
            Namespace.Global.Scope,
            null,
            "println",
            new ParameterType[] {
                 new ParameterType(null, paramType, "s")
            },
            initOverloads ? new Println[] {
                new Println(StringValueNode.Type),
                new Println(IntValueNode.Type),
                new Println(BoolValueNode.Type)
            } : null
        ) { }

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments)
        {
            var arg = arguments[0].Value;

            switch (arg)
            {
                case RuntimeClassInstanceValueNode civn:
                    var caster = civn.RuntimeScope.GetValue("!as_string") as RuntimeFunctionValueNode;

                    System.Console.Write((string)caster.Invoke(civn.RuntimeScope, arguments).Value);
                    break;

                default:
                    System.Console.Write(arg);
                    break;
            }

            return new RuntimeNullValueNode(DefiningToken);
        }
    }
}