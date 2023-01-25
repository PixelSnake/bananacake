using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Std {
    public class print : NativeFunctionType {
        public static NativeFunctionType Implementation = new print(IStringCast.IStringCast.Implementation, true);

        public override bool ExpectsThisArg => false;

        private print(Type typearg1, bool initOverloads = false) : base(
            Namespace.Global.Scope,
            null,
            "print",
            new ParameterType[] {
                 new ParameterType(null, typearg1, "p1"),
            },
            initOverloads ? new NativeFunctionType[]
            {
                new print(StringValueNode.Type),
            } : null
        ) {}

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
                        
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
